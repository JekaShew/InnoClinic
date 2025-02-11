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
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationAPI.Services.Services;

public class UserService : IUserService
{
    private readonly IValidator<OldNewPasswordPairDTO> _oldNewPasswordPairValidator;
    private readonly IValidator<EmailSecretPhrasePairDTO> _emailSecretPhrasePairValidator;
    private readonly IValidator<UserForUpdateDTO> _userForUpdateValidator;

    private readonly IRepositoryManager _repositoryManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserService(
            IHttpContextAccessor httpContextAccessor,
            IRepositoryManager repositoryManager,
            IValidator<OldNewPasswordPairDTO> oldNewPasswordPairValidator,
            IValidator<EmailSecretPhrasePairDTO> emailSecretPhrasePairValidator,
            IValidator<UserForUpdateDTO> userForUpdateValidator)
    {
        _httpContextAccessor = httpContextAccessor;
        _repositoryManager = repositoryManager;
        _oldNewPasswordPairValidator = oldNewPasswordPairValidator;
        _emailSecretPhrasePairValidator = emailSecretPhrasePairValidator;
        _userForUpdateValidator = userForUpdateValidator;
    }
    // Add methods ChangeOldEmailByEmail
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

        var defaultUserStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(DBConstants.ActivatedUserStatusId);
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
    //Implement
    public Task<ResponseMessage> ChangeEmail(string password, string newEmail)
    {
        //check currentUser??
        //check password
        //send activate code to newEmail
        throw new NotImplementedException();
    }
    //Implement
    public Task<ResponseMessage> ChangeForgottenPasswordByEmail(string email)
    {
        // send activate code to newEmail
        throw new NotImplementedException();
    }

    public async Task<ResponseMessage> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword)
    {
        var validationResult = await _emailSecretPhrasePairValidator.ValidateAsync(emailSecretPhrasePairDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var user = await _repositoryManager.User.GetUserByEmailAsync(emailSecretPhrasePairDTO.Email, true);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.CheckCredsMessage, false);
        }

        var enteredSecretPhraseHash = await GetHashString($"{emailSecretPhrasePairDTO.SecretPhrase}{user.SecurityStamp}");
        if (!enteredSecretPhraseHash.Equals(user.SecretPhraseHash))
        {
            return new ResponseMessage(MessageConstants.CheckCredsMessage, false);
        }
           
        user.PasswordHash = await GetHashString($"{newPassword}{user.SecurityStamp}");
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

    public async Task<ResponseMessage> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPairDTO)
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        var role = await _repositoryManager.Role.GetRoleByIdAsync(userIdRoleIdPairDTO.RoleId);
        if (role is null)
        {
            return new ResponseMessage(MessageConstants.CheckDBDataMessage, false);
        }

        var user = await _repositoryManager.User.GetUserByIdAsync(userIdRoleIdPairDTO.UserId, true);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }
          
        user.RoleId = userIdRoleIdPairDTO.RoleId;
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO)
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        var userStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(userIdUserStatusIdPairDTO.UserStatusId);
        if (userStatus is null)
        {
            return new ResponseMessage(MessageConstants.CheckDBDataMessage, false);
        }

        var user = await _repositoryManager.User.GetUserByIdAsync(userIdUserStatusIdPairDTO.UserId);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        user.UserStatusId = userIdUserStatusIdPairDTO.UserStatusId;
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }
}
