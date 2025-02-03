using AuthorizationAPI.Shared.DTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<CustomResponse<(string, string)>> SignIn(LoginInfoDTO loginInfoDTO);
        public Task<CustomResponse> SignOut(Guid rTokenId);
        public Task<CustomResponse<(string, string)>> SignUp(RegistrationInfoDTO registrationInfoDTO);
        public Task<CustomResponse<(string, string)>> Refresh(Guid rTokenId);
        public Task<CustomResponse> RevokeTokenByRefreshTokenId(Guid rTokenId);
    }
}
