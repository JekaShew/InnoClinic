using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces;

public interface IUserService
{
    public Task<ResponseMessage<List<UserInfoDTO>>> GetAllUsersInfo();
    public Task<ResponseMessage<UserDetailedDTO>> GetUserDetailedInfo(Guid userId);
    public Task<ResponseMessage> DeleteCurrentAccount();
    public Task<ResponseMessage> DeleteUserById(Guid userId);
    public Task<ResponseMessage> UpdateUserInfo(Guid userId, UserForUpdateDTO userForUpdateDTO);
    public Task<ResponseMessage> ChangePasswordByOldPassword(OldNewPasswordPairDTO oldNewPasswordPairDTO);
    public Task<ResponseMessage> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword);
    public Task<ResponseMessage> ChangeForgottenPasswordByEmail(string email);
    public Task<ResponseMessage> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO);
    public Task<ResponseMessage> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPirDTO);
   
    public Task<Guid> CreateUserAsync(RegistrationInfoDTO registrationInfoDTO);
    public Task<UserDetailedDTO> IsCurrentUserAdministrator();
    public Task<UserDetailedDTO> IsEmailRegistered(string email, bool trackChanges);
    public Task<string> GetHashString(string stringToHash);
}
