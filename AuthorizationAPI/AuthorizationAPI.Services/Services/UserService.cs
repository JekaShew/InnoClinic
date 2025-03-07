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
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return new ResponseMessage("Server's Error Occured! Check Initial Database Data!", 500);
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if(currentUserInfo is null)
        {
            return new ResponseMessage("Forbidden Action! You have no Rights to manage this Account!", 403);
        }    

        var currentUser = await _repositoryManager.User.GetUserByIdAsync(currentUserInfo.Id);
        currentUser.UserStatusId = deletedUserStatus.Id;    
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeleteUserById(Guid userId)
    {
        var userToDelete = await _repositoryManager.User.GetUserByIdAsync(userId);
        _repositoryManager.User.DeleteUser(userToDelete);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<IEnumerable<UserInfoDTO>>> GetAllUsersInfo()
    {
        var users = await _repositoryManager.User.GetAllUsersAsync();
        if (!users.Any())
        {
            return new ResponseMessage<IEnumerable<UserInfoDTO>>("No Users Found in Database!", 404);
        }

        var userInfoDTOs = users.Select(u => UserMapper.UserToUserInfoDTO(u)).ToList();

        return new ResponseMessage<IEnumerable<UserInfoDTO>>(userInfoDTOs);
    }

    public async Task<ResponseMessage<UserInfoDTO>> GetUserInfoById(Guid userId)
    {
        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage<UserInfoDTO>("User Not Found!", 404);
        }

        var userInfoDTO = UserMapper.UserToUserInfoDTO(user);

        return new ResponseMessage<UserInfoDTO>(userInfoDTO);
    }

    public async Task<ResponseMessage<UserDetailedDTO>> GetUserDetailedInfoById(Guid userId)
    {
        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage<UserDetailedDTO>("User Not Found!", 404);
        }

        var userDetailedInfoDTO = UserMapper.UserToUserDetailedDTO(user);

        return new ResponseMessage<UserDetailedDTO>(userDetailedInfoDTO);
    }

    public async Task<ResponseMessage> UpdateUserInfo(Guid userId, UserForUpdateDTO userForUpdateDTO)
    {
        var validationResult = await _userForUpdateValidator.ValidateAsync(userForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage("User Not Found!", 404);
        }

        if (!user.Id.Equals(currentUserInfo.Id))
        {
            return new ResponseMessage("Forbidden Action! You Have No Rights to Manage this Account!", 403);
        }

        UserMapper.UpdateUserFromUserForUpdateDTO(userForUpdateDTO,user);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
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
            return new ResponseMessage("User Not Found!", 404);
        }

        UserMapper.UpdateUserFromUserForUpdateByAdministratorDTO(userForUpdateByAdministratorDTO, user);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
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
            return new ResponseMessage("Forbidden Action! You Have No Rights to Manage this Account!", 403);
        }

        var user = await _repositoryManager.User.GetUserByIdAsync(currentUserInfo.Id);
        var enteredPasswordHash = await _commonService.GetHashString($"{oldNewPasswordPairDTO.OldPassword}{user.SecurityStamp}");
        if (!enteredPasswordHash.Equals(user.PasswordHash))
        {
            return new ResponseMessage("Access Denied! Check Credentials, you have entered!", 400);
        }

        user.PasswordHash = await _commonService.GetHashString($"{oldNewPasswordPairDTO.NewPassword}{user.SecurityStamp}");
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
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
            return new ResponseMessage("Access Denied! Check Email, you have entered!", 400);
        }

        var enteredSecretPhraseHash = await _commonService.GetHashString($"{emailSecretPhraseNewPasswordDTO.SecretPhrase}{user.SecurityStamp}");
        if (!enteredSecretPhraseHash.Equals(user.SecretPhraseHash))
        {
            return new ResponseMessage("Access Denied! Check Credentials, you have entered!", 400);
        }

        user.PasswordHash = await _commonService.GetHashString($"{emailSecretPhraseNewPasswordDTO.NewPassword}{user.SecurityStamp}");
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> ChangeUserStatusOfUser(Guid userId, JsonPatchDocument<UserForUpdateByAdministratorDTO> patchDocForUserInfoDTO)
    {
        var patchOperation = patchDocForUserInfoDTO.Operations.First().op;
        if (!patchOperation.Equals(PatchConstants.ReplaceOperation))
        {
            return new ResponseMessage("An Error Occured while Proccessing patch operation! Operation is incorrect!", 400);
        }

        Guid patchValueGuid = Guid.Empty;
        var patchValue = patchDocForUserInfoDTO.Operations.First().value.ToString();
        if (patchValue is null
            || !Guid.TryParse(patchValue, out patchValueGuid)
                || patchValueGuid.Equals(Guid.Empty))
        {
            return new ResponseMessage("An Error Occured while Proccessing patch operation! Value is inValid!", 400);
        }

        var userStatus = await _repositoryManager.UserStatus.GetUserStatusByIdAsync(patchValueGuid);
        if (userStatus is null)
        {
            return new ResponseMessage("Server's Error Occured! Check Initial Database Data!", 500);
        }

        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage("User Not Found!", 404);
        }

        var userforUpdateByAdministratorDTO = UserMapper.UserToUserForUpdateByAdministratorDTO(user);
        patchDocForUserInfoDTO.ApplyTo(userforUpdateByAdministratorDTO);
        UserMapper.UpdateUserFromUserForUpdateByAdministratorDTO(userforUpdateByAdministratorDTO, user);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> ChangeRoleOfUser(Guid userId, JsonPatchDocument<UserForUpdateByAdministratorDTO> patchDocForUserInfoDTO)
    {
        var patchOperation = patchDocForUserInfoDTO.Operations.First().op.ToString();
        if (!patchOperation.Equals(PatchConstants.ReplaceOperation))
        {
            return new ResponseMessage("An Error Occured while Proccessing patch operation! Operation is incorrect!", 400);
        }

        Guid patchValueGuid = Guid.Empty;
        var patchValue = patchDocForUserInfoDTO.Operations.First().value.ToString();
        if (patchValue is null
            || !Guid.TryParse(patchValue, out patchValueGuid)
                || patchValueGuid.Equals(Guid.Empty))
        {
            return new ResponseMessage("An Error Occured while Proccessing patch operation!Value is inValid!", 400);
        }

        var role = await _repositoryManager.Role.GetRoleByIdAsync(patchValueGuid);
        if (role is null)
        {
            return new ResponseMessage("Server's Error Occured! Check Initial Database Data!", 500);
        }

        var user = await _repositoryManager.User.GetUserByIdAsync(userId);
        if (user is null)
        {
            return new ResponseMessage("User Not Found!", 404);
        }

        var userforUpdateByAdministratorDTO = UserMapper.UserToUserForUpdateByAdministratorDTO(user);
        patchDocForUserInfoDTO.ApplyTo(userforUpdateByAdministratorDTO);
        UserMapper.UpdateUserFromUserForUpdateByAdministratorDTO(userforUpdateByAdministratorDTO, user);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> ActivateUser(string email, string token)
    {
       var confirmEmail = await ConfirmEmail(email, token);
        if (!confirmEmail.IsComplited)
        {
            return confirmEmail;
        }

        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if(user is null)
        {
            return new ResponseMessage("User Not Found!", 404);
        }

        user.UserStatusId = DBConstants.ActivatedUserStatusId;
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> ChangeForgottenPasswordByEmailRequest(string email)
    {
        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if(user is null)
        {
            return new ResponseMessage("User Not Found!", 404);
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
        if(!confirmEmail.IsComplited)
        {
            return confirmEmail;
        }

        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if(user is null)
        {
            return new ResponseMessage("User Not Found!", 404);
        }

        user.PasswordHash = await _commonService.GetHashString($"{newPassword}{user.SecurityStamp}");
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
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
            return new ResponseMessage("Access Denied!  You Have no Rights to manage this Account!", 403);
        }

        var currentUser = await _repositoryManager.User.GetUserByIdAsync(currentUserInfo.Id);
        if(currentUser is null)
        {
            return new ResponseMessage("User Not Found!", 404);
        }

        var enteredPasswordHash = await _commonService.GetHashString($"{emailPasswordPairDTO.Password}{currentUser.SecurityStamp}");
        if(!currentUser.PasswordHash.Equals(enteredPasswordHash))
        {
            return new ResponseMessage("Access Denied! Check Credentials, you have entered!", 400);
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
            return new ResponseMessage("User Not Found!", 404);
        }

        if (!_memoryCache.TryGetValue(email, out string? dateTimeString) || dateTimeString.IsNullOrEmpty())
        {
            return new ResponseMessage("Verification Token not Found!", 404);
        }
        var verificationToken = _emailService
                .GenerateEmailConfirmationTokenByEmailAndDateTime(email, dateTimeString!);

        if (!verificationToken.Equals(token))
        {
            return new ResponseMessage("Verification Error! Can't verify Your Token!", 500);
        }

        _memoryCache.Remove(email);

        return new ResponseMessage();
    } 
}
