using AuthorizationAPI.Shared.DTOs.UserDTOs;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.JsonPatch;

namespace AuthorizationAPI.Services.Abstractions.Interfaces;

public interface IUserService
{
    public Task<ResponseMessage<IEnumerable<UserInfoDTO>>> GetAllUsersInfo();
    public Task<ResponseMessage<UserDetailedDTO>> GetUserDetailedInfo(Guid userId);
    public Task<ResponseMessage> DeleteCurrentAccount();
    public Task<ResponseMessage> ActivateUser(string email,string token);
    public Task<ResponseMessage> DeleteUserById(Guid userId);
    public Task<ResponseMessage> UpdateUserInfo(Guid userId, UserForUpdateDTO userForUpdateDTO);
    public Task<ResponseMessage> ChangePasswordByOldPassword(OldNewPasswordPairDTO oldNewPasswordPairDTO);
    public Task<ResponseMessage> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhraseNewPasswordDTO emailSecretPhraseNewPasswordDTO);
    public Task<ResponseMessage> ChangeForgottenPasswordByEmailRequest(string email);
    public Task<ResponseMessage> ChangeForgottenPasswordByEmail(string token, string email, string newPassword);
    public Task<ResponseMessage> ChangeEmailByPassword(EmailPasswordPairDTO emailPasswordPairDTO);
    public Task<ResponseMessage> ChangeUserStatusOfUser(Guid userId, JsonPatchDocument<UserInfoDTO> patchDocForUserInfoDTO);
    public Task<ResponseMessage> ChangeRoleOfUser(Guid userId, JsonPatchDocument<UserInfoDTO> patchDocForUserInfoDTO);
   
    public Task<Guid> CreateUserAsync(RegistrationInfoDTO registrationInfoDTO);
    public Task<string> GetHashString(string stringToHash);
    public Guid? GetCurrentUserId();
}
