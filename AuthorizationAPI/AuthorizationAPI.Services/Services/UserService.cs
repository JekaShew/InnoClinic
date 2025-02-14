using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationAPI.Services.Services;

public class UserService : IUserService
{
    private readonly IValidator<OldNewPasswordPairDTO> _oldNewPasswordPairValidator;
    private readonly IValidator<EmailSecretPhraseNewPasswordDTO> _emailSecretPhraseNewPasswordValidator;
    private readonly IValidator<UserForUpdateDTO> _userForUpdateValidator;

    private readonly IRepositoryManager _repositoryManager;
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMemoryCache _memoryCache;
    public UserService(
            IHttpContextAccessor httpContextAccessor,
            IRepositoryManager repositoryManager,
            IValidator<OldNewPasswordPairDTO> oldNewPasswordPairValidator,
            IValidator<EmailSecretPhraseNewPasswordDTO> emailSecretPhraseNewPasswordValidator,
            IValidator<UserForUpdateDTO> userForUpdateValidator,
            IMemoryCache memoryCache,
            IAuthorizationService authorizationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _repositoryManager = repositoryManager;
        _oldNewPasswordPairValidator = oldNewPasswordPairValidator;
        _emailSecretPhraseNewPasswordValidator = emailSecretPhraseNewPasswordValidator;
        _userForUpdateValidator = userForUpdateValidator;
        _memoryCache = memoryCache;
        _authorizationService = authorizationService;
    }

    public Guid? GetCurrentUserId()
    {
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return Guid.Empty;
        }

        var claim = _httpContextAccessor
                    .HttpContext
                    .User
                    .Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

        if (claim is null)
        {
            return Guid.Empty;
        }

        return Guid.Parse(claim.Value);
    }

    public async Task<string> GetHashString(string stringToHash)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes($"{stringToHash}");
            var ms = new MemoryStream(inputBytes);
            var hashBytes = await md5.ComputeHashAsync(ms);
            var stringHash = Encoding.UTF8.GetString(hashBytes);

            return stringHash;
        }
    }

    public async Task<Guid> CreateUserAsync(RegistrationInfoDTO registrationInfoDTO)
    {
        var defaultRole = await _repositoryManager.Role.GetRoleByIdAsync(DBConstants.PatientRoleId);
        if (defaultRole is null || defaultRole.Id.Equals(Guid.Empty))
        {
            return Guid.Empty;
        }

        var defaultUserStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(DBConstants.NonActivatedUserStatusId);
        if (defaultUserStatus is null && defaultUserStatus.Equals(Guid.Empty))
        {
            return Guid.Empty;
        }

        var securityStamp = await GetHashString(registrationInfoDTO.SecretPhrase);
        var secretPhraseHash = await GetHashString($"{registrationInfoDTO.SecretPhrase}{securityStamp}");
        var passwordHash = await GetHashString($"{registrationInfoDTO.Password}{securityStamp}");

        var newUser = UserMapper.RegistrationInfoDTOToUser(registrationInfoDTO);
        newUser.RoleId = defaultRole.Id;
        newUser.UserStatusId = defaultUserStatus.Id;
        newUser.SecurityStamp = securityStamp;
        newUser.SecretPhraseHash = secretPhraseHash;
        newUser.PasswordHash = passwordHash;

        await _repositoryManager.User.CreateUserAsync(newUser);
        await _repositoryManager.CommitAsync();

        return newUser.Id;
    }

    public async Task<ResponseMessage> DeleteCurrentAccount()
    {
        var deletedUserStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(DBConstants.DeletedUserStatusId);
        if (deletedUserStatus is null || deletedUserStatus.Id.Equals(Guid.Empty))
        {
            return new ResponseMessage(MessageConstants.CheckDBDataMessage, false);
        }

        var currentUserId = GetCurrentUserId();
        var currentUser = await _repositoryManager.User.GetUserByIdAsync(currentUserId.Value, true);
        currentUser.UserStatusId = deletedUserStatus.Id;
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> DeleteUserById(Guid userId)
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        var userToDelete = await _repositoryManager.User.GetUserByIdAsync(userId);
        _repositoryManager.User.DeleteUser(userToDelete);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
    }

    public async Task<ResponseMessage<IEnumerable<UserInfoDTO>>> GetAllUsersInfo()
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage<IEnumerable<UserInfoDTO>>(MessageConstants.ForbiddenMessage, false);
        }

        var users = await _repositoryManager.User.GetAllUsersAsync(false);
        if (!users.Any())
        {
            return new ResponseMessage<IEnumerable<UserInfoDTO>>(MessageConstants.NotFoundMessage, false);
        }

        var userInfoDTOs = users.Select(u => UserMapper.UserToUserInfoDTO(u)).ToList();

        return new ResponseMessage<IEnumerable<UserInfoDTO>>(MessageConstants.SuccessMessage, true, userInfoDTOs);
    }

    public async Task<ResponseMessage<UserDetailedDTO>> GetUserDetailedInfo(Guid userId)
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage<UserDetailedDTO>(MessageConstants.ForbiddenMessage, false);
        }

        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage<UserDetailedDTO>(MessageConstants.NotFoundMessage, false);
        }

        var userDetailedInfoDTO = UserMapper.UserToUserDetailedDTO(user);

        return new ResponseMessage<UserDetailedDTO>(MessageConstants.SuccessMessage, true, userDetailedInfoDTO);
    }

    public async Task<ResponseMessage> UpdateUserInfo(Guid userId, UserForUpdateDTO userForUpdateDTO)
    {
        var validationResult = await _userForUpdateValidator.ValidateAsync(userForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserId = GetCurrentUserId();
        var user = await _repositoryManager.User.GetUserByIdAsync(userId, true);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        if (!user.Id.Equals(currentUserId.Value))
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        user = UserMapper.UserForUpdateDTOToUser(userForUpdateDTO);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> ChangePasswordByOldPassword(OldNewPasswordPairDTO oldNewPasswordPairDTO)
    {
        var validationResult = await _oldNewPasswordPairValidator.ValidateAsync(oldNewPasswordPairDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserId = GetCurrentUserId();
        var user = await _repositoryManager.User.GetUserByIdAsync(currentUserId.Value);
        var enteredPasswordHash = await GetHashString($"{oldNewPasswordPairDTO.OldPassword}{user.SecurityStamp}");
        if (!enteredPasswordHash.Equals(user.PasswordHash))
        {
            return new ResponseMessage(MessageConstants.CheckCredsMessage, false);
        }

        user.PasswordHash = await GetHashString($"{oldNewPasswordPairDTO.NewPassword}{user.SecurityStamp}");
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhraseNewPasswordDTO emailSecretPhraseNewPasswordDTO)
    {
        var validationResult = await _emailSecretPhraseNewPasswordValidator.ValidateAsync(emailSecretPhraseNewPasswordDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var user = await _repositoryManager.User.GetUserByEmailAsync(emailSecretPhraseNewPasswordDTO.Email, true);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.CheckCredsMessage, false);
        }

        var enteredSecretPhraseHash = await GetHashString($"{emailSecretPhraseNewPasswordDTO.SecretPhrase}{user.SecurityStamp}");
        if (!enteredSecretPhraseHash.Equals(user.SecretPhraseHash))
        {
            return new ResponseMessage(MessageConstants.CheckCredsMessage, false);
        }

        user.PasswordHash = await GetHashString($"{emailSecretPhraseNewPasswordDTO.NewPassword}{user.SecurityStamp}");
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> ChangeUserStatusOfUser(Guid userId, JsonPatchDocument<UserInfoDTO> patchDocForUserInfoDTO)
    {
        var patchOperation = patchDocForUserInfoDTO.Operations.First().op;
        if (!patchOperation.Equals(PatchConstants.ReplaceOperation))
        {
            return new ResponseMessage(MessageConstants.FailedMessage, false);
        }

        var currentUserId = GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        Guid patchValueGuid = Guid.Empty;
        var patchValue = patchDocForUserInfoDTO.Operations.First().value.ToString();
        if (patchValue is null
            || !Guid.TryParse(patchValue, out patchValueGuid)
                || patchValueGuid.Equals(Guid.Empty))
        {
            return new ResponseMessage(MessageConstants.FailedMessage, false);
        }

        var userStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(patchValueGuid);
        if (userStatus is null)
        {
            return new ResponseMessage(MessageConstants.CheckDBDataMessage, false);
        }

        var user = await _repositoryManager.User.GetUserByIdAsync(userId, true);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        var userInfoDTO = UserMapper.UserToUserInfoDTO(user);
        patchDocForUserInfoDTO.ApplyTo(userInfoDTO);
        user = UserMapper.UserInfoDTOToUser(userInfoDTO);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> ChangeRoleOfUser(Guid userId, JsonPatchDocument<UserInfoDTO> patchDocForUserInfoDTO)
    {
        var patchOperation = patchDocForUserInfoDTO.Operations.First().op;
        if (patchOperation.Equals(PatchConstants.ReplaceOperation))
        {
            return new ResponseMessage(MessageConstants.FailedMessage, false);
        }

        var currentUserId = GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        Guid patchValueGuid = Guid.Empty;
        var patchValue = patchDocForUserInfoDTO.Operations.First().value.ToString();
        if (patchValue is null
            || !Guid.TryParse(patchValue, out patchValueGuid)
                || patchValueGuid.Equals(Guid.Empty))
        {
            return new ResponseMessage(MessageConstants.FailedMessage, false);
        }

        var role = await _repositoryManager.Role.GetRoleByIdAsync(patchValueGuid);
        if (role is null)
        {
            return new ResponseMessage(MessageConstants.CheckDBDataMessage, false);
        }

        var user = await _repositoryManager.User.GetUserByIdAsync(userId, true);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        var userInfoDTO = UserMapper.UserToUserInfoDTO(user);
        patchDocForUserInfoDTO.ApplyTo(userInfoDTO);
        user = UserMapper.UserInfoDTOToUser(userInfoDTO);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> ActivateUser(string email, string token)
    {
       var confirmEmail = await ConfirmEmail(email, token);
        if (!confirmEmail.Flag)
        {
            return confirmEmail;
        }

        var user = await _repositoryManager.User.GetUserByEmailAsync(email, true);
        if(user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        user.UserStatusId = DBConstants.ActivatedUserStatusId;
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);

    }

    private async Task<ResponseMessage> ConfirmEmail(string email, string token)
    {
        var isRegistered = await _repositoryManager.User.IsEmailRegistered(email);
        if (!isRegistered)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        if (!_memoryCache.TryGetValue(email, out string? dateTimeString) || dateTimeString.IsNullOrEmpty())
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }
        var verificationToken = _authorizationService
                .GenerateEmailConfirmationTokenByUserIdAndCurrentDateTime(email, dateTimeString!);

        if (!verificationToken.Equals(token))
        {
            return new ResponseMessage(MessageConstants.FailedMessage, false);
        }

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public Task<ResponseMessage> ChangeForgottenPasswordByEmailRequest(string email)
    {
        // send activate code to newEmail
        throw new NotImplementedException();
    }

    public async Task<ResponseMessage> ChangeForgottenPasswordByEmail(string token, string email, string newPassword)
    {
        var confirmEmail = await ConfirmEmail(email, token);
        if(!confirmEmail.Flag)
        {
            return confirmEmail;
        }

        var user = await _repositoryManager.User.GetUserByEmailAsync(email,true);
        if(user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        user.PasswordHash = await GetHashString($"{newPassword}{user.SecurityStamp}");
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessUpdateMessage, true);
    }

    public async Task<ResponseMessage> ChangeEmailByPassword(EmailPasswordPairDTO emailPasswordPairDTO)
    {
        //check currentUser
        //check password
        //send activate code to newEmail
        var userId = GetCurrentUserId();

        var currentUser = await _repositoryManager.User.GetUserByIdAsync(userId.Value,true);
        if(currentUser is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        var enteredPasswordHash = await GetHashString($"{emailPasswordPairDTO.Password}{currentUser.SecurityStamp}");
        if(!currentUser.PasswordHash.Equals(enteredPasswordHash))
        {
            return new ResponseMessage(MessageConstants.CheckCredsMessage, false);
        }

        currentUser.Email = emailPasswordPairDTO.NewEmail;
        currentUser.UserStatusId = DBConstants.NonActivatedUserStatusId;

        // send email confirm to new Email
    }
}
