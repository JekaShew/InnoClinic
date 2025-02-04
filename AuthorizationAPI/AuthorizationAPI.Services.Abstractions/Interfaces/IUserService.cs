using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces
{
    public interface IUserService
    {
        public Task<Guid> CreateUserAsync(RegistrationInfoDTO registrationInfoDTO);
        public Task<UserDetailedDTO> IsEmailRegistered(string email, bool trackChanges);
        public Task<UserDetailedDTO> IsCurrentUserAdministrator();
        public Task<CommonResponse> DeleteAccountById();
        public Task<CommonResponse> DeleteUserById(Guid userId);
        public Task<CommonResponse<List<UserInfoDTO>>> TakeAllUsersInfo();
        public Task<CommonResponse<UserDetailedDTO>> GetUserDetailedInfo(Guid userId);
        public Task<CommonResponse> UpdateUserInfo(UserInfoDTO userInfoDTO);
        public Task<CommonResponse> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO);
        public Task<CommonResponse> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPirDTO);
        public Task<CommonResponse> ChangePasswordByOldPassword(string oldPassword, string newPassword);
        public Task<CommonResponse> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword);
        public Task<CommonResponse> ChangeForgottenPasswordByEmail(string email);
        public Task<string> GetHashString(string stringToHash);

    }
}
