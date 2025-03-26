using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Extensions;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using CommonLibrary.CommonService;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthorizationAPI.Services.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IValidator<LoginInfoDTO> _loginInfoValidator;
    private readonly IValidator<RegistrationInfoDTO> _registrationInfoValidator;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ICommonService _commonService;
    private readonly IEmailService _emailService;
    private readonly AuthorizationJWTSettings _authorizationSettings;

    public AuthorizationService(
            IOptions<AuthorizationJWTSettings> options,
            IRepositoryManager repositoryManager,
            ICommonService commonService,
            IValidator<LoginInfoDTO> loginInfoValidator,
            IValidator<RegistrationInfoDTO> registrationInfoValidator,
            IEmailService emailService)
    {
        _authorizationSettings = options.Value;
        _repositoryManager = repositoryManager;
        _commonService = commonService;
        _emailService = emailService;
        _loginInfoValidator = loginInfoValidator;
        _registrationInfoValidator = registrationInfoValidator;
    }

    public async Task<ResponseMessage<TokensDTO>> SignIn(LoginInfoDTO loginInfoDTO)
    {
        var validationResult = await _loginInfoValidator.ValidateAsync(loginInfoDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        //Check Email Registered
        var user = await _repositoryManager.User.GetUserByEmailAsync(loginInfoDTO.Email);
        if (user is null)
        {
            return new ResponseMessage<TokensDTO>("Access Denied! Check Email, you have entered!", 400);
        }

        if(!user.UserStatusId.Equals(DBConstants.ActivatedUserStatusId))
        {
            return new ResponseMessage<TokensDTO>("Forbidden Action! Your user status is incorrect!", 403);
        }

        //Check Email Password pair
        var enteredPasswordHash = await _commonService.GetHashString($"{loginInfoDTO.Password}{user.SecurityStamp}");
        if (!enteredPasswordHash.Equals(user.PasswordHash))
        {
            return new ResponseMessage<TokensDTO>("Access Denied! Check Credentials, you have entered!", 400);
        }  

        //Generate A&R Tokens
        var tokens = await GenerateTokenPair(user.Id);
        if(tokens.AccessToken.IsNullOrEmpty() || tokens.RefreshToken.IsNullOrEmpty())
        {
            return new ResponseMessage<TokensDTO>("Server's Error Occured!", 500);
        }

        return new ResponseMessage<TokensDTO>(tokens);
    }

    public async Task<ResponseMessage> SignOut(Guid refreshTokenId)
    {
        var refreshToken = await _repositoryManager.RefreshToken.GetRefreshTokenByIdAsync(refreshTokenId);
        if (refreshToken is null)
        {
            return new ResponseMessage("No Refrest Token Found!", 404);
        } 

        _repositoryManager.RefreshToken.DeleteRefreshToken(refreshToken);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<Guid>> SignUp(RegistrationInfoDTO registrationInfoDTO)
    {
        var validationResult = await _registrationInfoValidator.ValidateAsync(registrationInfoDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        // Check Email Registered
        var user = await _repositoryManager.User.GetUserByEmailAsync(registrationInfoDTO.Email);
        if(user is not null)
        {
            return new ResponseMessage<Guid>("Server's Error Occured!", 500);
        }    

        // Create User 
        var defaultRole = await _repositoryManager.Role.GetRoleByIdAsync(DBConstants.PatientRoleId);
        if (defaultRole is null || defaultRole.Id.Equals(Guid.Empty))
        {
            return new ResponseMessage<Guid>("Server's Error Occured! Check Initial Database Data!", 500);
        }

        var defaultUserStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(DBConstants.NonActivatedUserStatusId);
        if (defaultUserStatus is null || defaultUserStatus.Equals(Guid.Empty))
        {
            return new ResponseMessage<Guid>("Server's Error Occured! Check Initial Database Data!", 500);
        }

        var securityStamp = await _commonService.GetHashString(registrationInfoDTO.SecretPhrase);
        var secretPhraseHash = await _commonService.GetHashString($"{registrationInfoDTO.SecretPhrase}{securityStamp}");
        var passwordHash = await _commonService.GetHashString($"{registrationInfoDTO.Password}{securityStamp}");

        var newUser = UserMapper.RegistrationInfoDTOToUser(registrationInfoDTO);
        newUser.RoleId = defaultRole.Id;
        newUser.UserStatusId = defaultUserStatus.Id;
        newUser.SecurityStamp = securityStamp;
        newUser.SecretPhraseHash = secretPhraseHash;
        newUser.PasswordHash = passwordHash;

        var userId = await _repositoryManager.User.CreateUserAsync(newUser);
        await _repositoryManager.CommitAsync();
        
        await _emailService.SendVerificationLetterToEmail(registrationInfoDTO.Email);

        return new ResponseMessage<Guid>(userId);
    }
    
    public async Task<ResponseMessage<TokensDTO>> Refresh(Guid rTokenId)
    {
        var refreshToken = await IsRefreshTokeCorrect(rTokenId, false);
        if (refreshToken is null)
        {
            return new ResponseMessage<TokensDTO>("Access Denied! Your Session is inValid", 403);
        }    
        //Generate A&R Tokens
        var tokens = await GenerateTokenPair(refreshToken.UserId);

        return new ResponseMessage<TokensDTO>(tokens);
    }

    public async Task<ResponseMessage> ResendEmailVerification(LoginInfoDTO loginInfoDTO)
    {
        var validationResult = await _loginInfoValidator.ValidateAsync(loginInfoDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }
        // Check Email Registered
        var user = await _repositoryManager.User.GetUserByEmailAsync(loginInfoDTO.Email);
        if (user is null)
        {
            return new ResponseMessage<TokensDTO>("Access Denied! Check Email, you have entered!", 400);
        }

        var enteredPasswordhash = await _commonService.GetHashString($"{loginInfoDTO.Password}{user.SecurityStamp}");
        if (!enteredPasswordhash.Equals(user.PasswordHash))
        {
            return new ResponseMessage("Access Denied! Check Credentials, you have entered!", 400);
        }

        return await _emailService.SendVerificationLetterToEmail(loginInfoDTO.Email);
    }

    private async Task<RefreshToken?> IsRefreshTokeCorrect(Guid rTokenId, bool trackChanges)
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

    private async Task<string?> GenerateJwtTokenStringByUserId(Guid userId)
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
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authorizationSettings.SecretKey));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _authorizationSettings.Issuer,
            Audience = _authorizationSettings.Audience,
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
