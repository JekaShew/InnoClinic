using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;

namespace AuthorizationAPI.Application.Interfaces
{
    public interface IAuthorizationServices
    {
        public Task<(CustomResponse, string, string)> SignIn(LoginInfoDTO loginInfoDTO);
        public Task<CustomResponse> SignOut(Guid rTokenId);
        public Task<(CustomResponse, string, string)> SignUp(RegistrationInfoDTO registrationInfoDTO);
        public Task<(CustomResponse, string, string)> Refresh(Guid rTokenId);
        public Task<CustomResponse> RevokeTokenByRTokenId(Guid rTokenId);
    }
}
