using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using CommonLibrary.CommonService;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationAPI.Services.Services;

public class UserService : IUserService
{
    private readonly IValidator<OldNewPasswordPairDTO> _oldNewPasswordPairValidator;
    private readonly IValidator<EmailSecretPhraseNewPasswordDTO> _emailSecretPhraseNewPasswordValidator;
    private readonly IValidator<UserForUpdateDTO> _userForUpdateValidator;
    private readonly IValidator<UserForUpdateByAdministratorDTO> _userForUpdateByAdministratorValidator;
    private readonly IValidator<EmailPasswordPairDTO> _emailPasswordPairValidator;

    private readonly IRepositoryManager _repositoryManager;
    private readonly ICommonService _commonService;
    private readonly IEmailService _emailService;
    private readonly IMemoryCache _memoryCache;
    public UserService(
            IRepositoryManager repositoryManager,
            ICommonService commonService,
            IMemoryCache memoryCache,
            IEmailService emailService,
            IValidator<OldNewPasswordPairDTO> oldNewPasswordPairValidator,
            IValidator<EmailSecretPhraseNewPasswordDTO> emailSecretPhraseNewPasswordValidator,
            IValidator<UserForUpdateDTO> userForUpdateValidator,
            IValidator<EmailPasswordPairDTO> emailPasswordPairValidator,
            IValidator<UserForUpdateByAdministratorDTO> userForUpdateByAdministratorValidator)
    {
        _repositoryManager = repositoryManager;
        _commonService = commonService;
        _memoryCache = memoryCache;
        _emailService = emailService;
        _oldNewPasswordPairValidator = oldNewPasswordPairValidator;
        _emailSecretPhraseNewPasswordValidator = emailSecretPhraseNewPasswordValidator;
        _userForUpdateValidator = userForUpdateValidator;
        _emailPasswordPairValidator = emailPasswordPairValidator;
        _userForUpdateByAdministratorValidator = userForUpdateByAdministratorValidator;
    }

    public async Task<ResponseMessage> DeleteCurrentAccount()
    {
        var deletedUserStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(DBConstants.DeletedUserStatusId);
        if (deletedUserStatus is null || deletedUserStatus.Id.Equals(Guid.Empty))
        {
            return new ResponseMessage(MessageConstants.CheckDBDataMessage, false);
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if(currentUserInfo is null)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }    

        var currentUser = await _repositoryManager.User.GetUserByIdAsync(currentUserInfo.Id);
        currentUser.UserStatusId = deletedUserStatus.Id;    
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> DeleteUserById(Guid userId)
    {
        var userToDelete = await _repositoryManager.User.GetUserByIdAsync(userId);
        _repositoryManager.User.DeleteUser(userToDelete);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
    }

    public async Task<ResponseMessage<IEnumerable<UserInfoDTO>>> GetAllUsersInfo()
    {
        var users = await _repositoryManager.User.GetAllUsersAsync();
        if (!users.Any())
        {
            return new ResponseMessage<IEnumerable<UserInfoDTO>>(MessageConstants.NotFoundMessage, false);
        }

        var userInfoDTOs = users.Select(u => UserMapper.UserToUserInfoDTO(u)).ToList();

        return new ResponseMessage<IEnumerable<UserInfoDTO>>(MessageConstants.SuccessMessage, true, userInfoDTOs);
    }

    public async Task<ResponseMessage<UserInfoDTO>> GetUserInfoById(Guid userId)
    {
        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage<UserInfoDTO>(MessageConstants.NotFoundMessage, false);
        }

        var userInfoDTO = UserMapper.UserToUserInfoDTO(user);

        return new ResponseMessage<UserInfoDTO>(MessageConstants.SuccessMessage, true, userInfoDTO);
    }

    public async Task<ResponseMessage<UserDetailedDTO>> GetUserDetailedInfoById(Guid userId)
    {
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

        var currentUserId = _commonService.GetCurrentUserInfo();
        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        if (!user.Id.Equals(currentUserId))
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        UserMapper.UpdateUserFromUserForUpdateDTO(userForUpdateDTO,user);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> UpdateUserInfoByAdministrator(Guid userId, UserForUpdateByAdministratorDTO userForUpdateByAdministratorDTO)
    {
        var validationResult = await _userForUpdateByAdministratorValidator.ValidateAsync(userForUpdateByAdministratorDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        UserMapper.UpdateUserFromUserForUpdateByAdministratorDTO(userForUpdateByAdministratorDTO, user);
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

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if(currentUserInfo is null)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        var user = await _repositoryManager.User.GetUserByIdAsync(currentUserInfo.Id);
        var enteredPasswordHash = await _commonService.GetHashString($"{oldNewPasswordPairDTO.OldPassword}{user.SecurityStamp}");
        if (!enteredPasswordHash.Equals(user.PasswordHash))
        {
            return new ResponseMessage(MessageConstants.CheckCredsMessage, false);
        }

        user.PasswordHash = await _commonService.GetHashString($"{oldNewPasswordPairDTO.NewPassword}{user.SecurityStamp}");
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

        var user = await _repositoryManager.User.GetUserByEmailAsync(emailSecretPhraseNewPasswordDTO.Email);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.CheckCredsMessage, false);
        }

        var enteredSecretPhraseHash = await _commonService.GetHashString($"{emailSecretPhraseNewPasswordDTO.SecretPhrase}{user.SecurityStamp}");
        if (!enteredSecretPhraseHash.Equals(user.SecretPhraseHash))
        {
            return new ResponseMessage(MessageConstants.CheckCredsMessage, false);
        }

        user.PasswordHash = await _commonService.GetHashString($"{emailSecretPhraseNewPasswordDTO.NewPassword}{user.SecurityStamp}");
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> ChangeUserStatusOfUser(Guid userId, JsonPatchDocument<UserForUpdateByAdministratorDTO> patchDocForUserInfoDTO)
    {
        var patchOperation = patchDocForUserInfoDTO.Operations.First().op;
        if (!patchOperation.Equals(PatchConstants.ReplaceOperation))
        {
            return new ResponseMessage(MessageConstants.FailedMessage, false);
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

        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        var userforUpdateByAdministratorDTO = UserMapper.UserToUserForUpdateByAdministratorDTO(user);
        patchDocForUserInfoDTO.ApplyTo(userforUpdateByAdministratorDTO);
        UserMapper.UpdateUserFromUserForUpdateByAdministratorDTO(userforUpdateByAdministratorDTO, user);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> ChangeRoleOfUser(Guid userId, JsonPatchDocument<UserForUpdateByAdministratorDTO> patchDocForUserInfoDTO)
    {
        var patchOperation = patchDocForUserInfoDTO.Operations.First().op.ToString();
        if (!patchOperation.Equals(PatchConstants.ReplaceOperation))
        {
            return new ResponseMessage(MessageConstants.FailedMessage, false);
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

        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        var userforUpdateByAdministratorDTO = UserMapper.UserToUserForUpdateByAdministratorDTO(user);
        patchDocForUserInfoDTO.ApplyTo(userforUpdateByAdministratorDTO);
        UserMapper.UpdateUserFromUserForUpdateByAdministratorDTO(userforUpdateByAdministratorDTO, user);
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

        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if(user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        user.UserStatusId = DBConstants.ActivatedUserStatusId;
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage> ChangeForgottenPasswordByEmailRequest(string email)
    {
        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if(user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        return await _emailService.SendVerificationLetterToEmail(email);
    }

    public async Task<ResponseMessage> ChangeForgottenPasswordByEmail(string token, string email, string newPassword)
    {
        var validationResult = await _emailPasswordPairValidator.ValidateAsync(new EmailPasswordPairDTO() { NewEmail = email, Password = newPassword});
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var confirmEmail = await ConfirmEmail(email, token);
        if(!confirmEmail.Flag)
        {
            return confirmEmail;
        }

        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if(user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        user.PasswordHash = await _commonService.GetHashString($"{newPassword}{user.SecurityStamp}");
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessUpdateMessage, true);
    }

    public async Task<ResponseMessage> ChangeEmailByPassword(EmailPasswordPairDTO emailPasswordPairDTO)
    {
        var validationResult = await _emailPasswordPairValidator.ValidateAsync(emailPasswordPairDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        var currentUser = await _repositoryManager.User.GetUserByIdAsync(currentUserInfo.Id);
        if(currentUser is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        var enteredPasswordHash = await _commonService.GetHashString($"{emailPasswordPairDTO.Password}{currentUser.SecurityStamp}");
        if(!currentUser.PasswordHash.Equals(enteredPasswordHash))
        {
            return new ResponseMessage(MessageConstants.CheckCredsMessage, false);
        }

        currentUser.Email = emailPasswordPairDTO.NewEmail;
        currentUser.UserStatusId = DBConstants.NonActivatedUserStatusId;
        await _repositoryManager.CommitAsync();

        return await _emailService.SendVerificationLetterToEmail(emailPasswordPairDTO.NewEmail);
    }

    private async Task<ResponseMessage> ConfirmEmail(string email, string token)
    {
        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if(user is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        if (!_memoryCache.TryGetValue(email, out string? dateTimeString) || dateTimeString.IsNullOrEmpty())
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }
        var verificationToken = _emailService
                .GenerateEmailConfirmationTokenByEmailAndDateTime(email, dateTimeString!);

        if (!verificationToken.Equals(token))
        {
            return new ResponseMessage(MessageConstants.FailedMessage, false);
        }

        _memoryCache.Remove(email);

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    } 
}
