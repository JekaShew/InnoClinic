using AuthorizationAPI.Shared.DTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces
{
    public interface IUserServices
    {
        public Task<CustomResponse> CreateUser(RegistrationInfoDTO registrationInfoDTO);
        public Task<CustomResponse> DeleteAccountById();
        public Task<CustomResponse> DeleteUserById(Guid userId);
        public Task<CustomResponse<List<UserInfoDTO>>> TakeAllUsersInfo();
        public Task<CustomResponse<UserDetailedDTO>> GetUserDetailedInfo(Guid userId);
        public Task<CustomResponse> EditUserInfo(UserInfoDTO userInfoDTO);
        public Task<CustomResponse> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO);
        public Task<CustomResponse> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPirDTO);
        public Task<CustomResponse> ChangePasswordByOldPassword(string oldPassword, string newPassword);
        public Task<CustomResponse> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword);
        public Task<CustomResponse> ChangeForgottenPasswordByEmail(string email);
        public Task<string> GetHashString(string stringToHash);

    }
}
