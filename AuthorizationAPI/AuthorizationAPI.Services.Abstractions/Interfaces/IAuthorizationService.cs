using AuthorizationAPI.Shared.DTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<CommonResponse<(string, string)>> SignIn(LoginInfoDTO loginInfoDTO);
        public Task<CommonResponse> SignOut(Guid rTokenId);
        public Task<CommonResponse<(string, string)>> SignUp(RegistrationInfoDTO registrationInfoDTO);
        public Task<CommonResponse<(string, string)>> Refresh(Guid rTokenId);
        public Task<CommonResponse> RevokeTokenByRefreshTokenId(Guid rTokenId);
    }
}
