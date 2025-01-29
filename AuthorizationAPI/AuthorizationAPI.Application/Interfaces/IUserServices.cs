using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;

namespace AuthorizationAPI.Application.Interfaces
{
    public interface IUserServices
    {
        public Task<CustomResponse> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO);
        public Task<CustomResponse> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPirDTO);
        public Task<CustomResponse> ChangePasswordByOldPassword(string oldPassword, string newPassword);
        public Task<CustomResponse> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword);
        public Task<CustomResponse> ChangeForgottenPasswordByEmail(string email);
        public Task<string> GetHashString(string stringToHash);

    }
}
