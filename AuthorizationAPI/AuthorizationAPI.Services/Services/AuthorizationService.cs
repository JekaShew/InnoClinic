using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Extensions;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace AuthorizationAPI.Services.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IValidator<LoginInfoDTO> _loginInfoValidator;
    private readonly IValidator<RegistrationInfoDTO> _registrationInfoValidator;

    private readonly IRepositoryManager _repositoryManager;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    private readonly IMemoryCache _memoryCache;
    private readonly AuthorizationJWTSettings _authenticationSettings;

    public AuthorizationService(
            IOptions<AuthorizationJWTSettings> options,
            IRepositoryManager repositoryManager,
            IUserService userService,
            IValidator<LoginInfoDTO> loginInfoValidator,
            IValidator<RegistrationInfoDTO> registrationInfoValidator,
            IEmailService emailService,
            IMemoryCache memoryCache)
    {
        _authenticationSettings = options.Value;
        _repositoryManager = repositoryManager;
        _userService = userService;
        _loginInfoValidator = loginInfoValidator;
        _registrationInfoValidator = registrationInfoValidator;
        _emailService = emailService;
        _memoryCache = memoryCache;
    }

    public async Task<ResponseMessage<TokensDTO>> SignIn(LoginInfoDTO loginInfoDTO)
    {
        var validationResult = await _loginInfoValidator.ValidateAsync(loginInfoDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        //Check Email Registered
        var isEmailRegistered = await _repositoryManager.User.IsEmailRegistered(loginInfoDTO.Email);
        if (!isEmailRegistered)
        {
            return new ResponseMessage<TokensDTO>(MessageConstants.CheckCredsMessage, false);
        }

        var user = await _repositoryManager.User.GetUserByEmailAsync(loginInfoDTO.Email);
        if (user is null)
        {
            return new ResponseMessage<TokensDTO>(MessageConstants.CheckCredsMessage, false);
        }

        if(user.UserStatusId.Equals(DBConstants.DeletedUserStatusId) 
            || user.UserStatusId.Equals(DBConstants.BannedUserStatusId))
        {
            return new ResponseMessage<TokensDTO>(MessageConstants.ForbiddenMessage, false);
        }
        //Check Email Password pair
        var enteredPasswordHash = await _userService.GetHashString($"{loginInfoDTO.Password}{user.SecurityStamp}");
        if (!enteredPasswordHash.Equals(user.PasswordHash))
        {
            return new ResponseMessage<TokensDTO>(MessageConstants.CheckCredsMessage, false);
        }  

        //Generate A&R Tokens
        var tokens = await GenerateTokenPair(user.Id);
        if(tokens.AccessToken.IsNullOrEmpty() || tokens.RefreshToken.IsNullOrEmpty())
        {
            return new ResponseMessage<TokensDTO>(MessageConstants.FailedMessage, false);
        }

        return new ResponseMessage<TokensDTO>(MessageConstants.SuccessMessage, true, tokens);
    }

    public async Task<ResponseMessage> SignOut(Guid refreshTokenId)
    {
        var refreshToken = await _repositoryManager.RefreshToken.GetRefreshTokenByIdAsync(refreshTokenId);
        if (refreshToken is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        } 

        _repositoryManager.RefreshToken.DeleteRefreshToken(refreshToken);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> SignUp(RegistrationInfoDTO registrationInfoDTO)
    {
        var validationResult = await _registrationInfoValidator.ValidateAsync(registrationInfoDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        // Check Email Registered
        var isEmailRegistered = await _repositoryManager.User.IsEmailRegistered(registrationInfoDTO.Email);
        if (isEmailRegistered)
        {
            return new ResponseMessage(MessageConstants.EmailRegisteredMessage, false);
        }

        // Create User 
        var userId = await _userService.CreateUserAsync(registrationInfoDTO);
        if (userId.Equals(Guid.Empty))
        {
            return new ResponseMessage(MessageConstants.FailedCreateMessage, false);
        }

        // Setup Confirmation Message with confirm Link 
        var currentDateTimeString = DateTime.UtcNow.ToString();
        var confirmEmailToken = GenerateEmailConfirmationTokenByEmailAndDateTime(registrationInfoDTO.Email, currentDateTimeString);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSize(1)
            .SetPriority(CacheItemPriority.High);
        _memoryCache.Set(registrationInfoDTO.Email, currentDateTimeString, cacheEntryOptions);

        var callbackParameters = new Dictionary<string, string>
        {
            {"token", confirmEmailToken },
            {"email", registrationInfoDTO.Email}
        };
        // how to grab domain address and url of action to confirmEmail
        var callback = QueryHelpers.AddQueryString("http://localhost:5000/api/Users/verifyemail", callbackParameters);

        // Send Email
        var verificationEmailMetadata = new EmailMetadata(
                registrationInfoDTO.Email,
                EmailTemplates.VerificationTemplate.Key,
                $"{EmailTemplates.VerificationTemplate.Value} <a href='{HtmlEncoder.Default.Encode(callback)}'>{callback}</a>.");
        var sendEmail = await _emailService.SendSingleMail(verificationEmailMetadata); 
        if(!sendEmail)
        {
            return new ResponseMessage(MessageConstants.FailEmailVerificationMessage ,false);
        }

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }
    
    public async Task<ResponseMessage<TokensDTO>> Refresh(Guid rTokenId)
    {
        var refreshToken = await IsRefreshTokeCorrect(rTokenId, false);
        if (refreshToken is null)
        {
            return new ResponseMessage<TokensDTO>(MessageConstants.FailedMessage, false);
        }
           
        //Generate A&R Tokens
        var tokens = await GenerateTokenPair(refreshToken.UserId);

        return new ResponseMessage<TokensDTO>(MessageConstants.SuccessMessage, true, tokens);
    }

    public string GenerateEmailConfirmationTokenByEmailAndDateTime(string email, string dateTimeString)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{email}{dateTimeString}"));
        var emailConfirmationToken = Convert.ToBase64String(symmetricSecurityKey.Key);

        return emailConfirmationToken;
    }

    //private async Task<bool> SendEmail(string email, string subject, string message)
    //{
    //    // Send Email
    //    MailMessage mailMessage = new MailMessage();
    //    SmtpClient smtpClient = new SmtpClient();
    //    mailMessage.From = new MailAddress("");
    //    mailMessage.To.Add(email);
    //    mailMessage.Subject = subject;
    //    mailMessage.IsBodyHtml = true;
    //    mailMessage.Body = message;

    //    smtpClient.Port = 25;
    //    smtpClient.Host = "localhost";

    //    smtpClient.EnableSsl = false;
    //    smtpClient.UseDefaultCredentials = false;
    //    smtpClient.Credentials = new NetworkCredential("" ,"");
    //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

    //    smtpClient.Send(mailMessage);
    //    return true;
    //}

    private async Task<RefreshToken> IsRefreshTokeCorrect(Guid rTokenId, bool trackChanges)
    {
        //Check is RefreshToken Correct
        var refreshToken = await _repositoryManager.RefreshToken.GetRefreshTokenByIdAsync(rTokenId);
        if (refreshToken is null)
        {
            return null;
        }    

        if (refreshToken.IsRevoked)
        {
            return null;
        }   

        if (refreshToken.ExpireDate <= DateTime.UtcNow
                || refreshToken.ExpireDate.Equals(DateTime.MinValue))
        {
            return null;
        }

        return refreshToken;
    }

    private async Task<string> GenerateJwtTokenStringByUserId(Guid userId)
    {
        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        var role = await _repositoryManager.Role.GetRoleByIdAsync(user.RoleId);
        if (user is null || role is null || !user.UserStatusId.Equals(DBConstants.ActivatedUserStatusId))
        {
            return null;
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role.Title),
        };

        var jwtHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.SecretKey));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _authenticationSettings.Issuer,
            Audience = _authenticationSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials =
                new SigningCredentials(key,
                    SecurityAlgorithms.HmacSha256Signature)
        };

        var jwtToken = jwtHandler.CreateToken(tokenDescriptor);
        var tokenString = jwtHandler.WriteToken(jwtToken);

        return tokenString;
    }

    private async Task<string> GenerateRefreshTokenByUserId(Guid userId)
    {
        var refreshToken = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            IsRevoked = false,
            ExpireDate = DateTime.UtcNow.AddMinutes(120),
        };

        await _repositoryManager.RefreshToken.CreateRefreshTokenAsync(refreshToken);
        await _repositoryManager.CommitAsync();

        return refreshToken.Id.ToString();
    }

    private async Task<TokensDTO> GenerateTokenPair(Guid userId)
    {
        var jwt = await GenerateJwtTokenStringByUserId(userId);
        var rt = await GenerateRefreshTokenByUserId(userId);

        return new TokensDTO() { AccessToken = jwt, RefreshToken = rt };
    }
}
