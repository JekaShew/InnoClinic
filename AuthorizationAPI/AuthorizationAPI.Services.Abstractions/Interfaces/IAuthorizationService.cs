using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<CommonResponse<TokensDTO>> SignIn(LoginInfoDTO loginInfoDTO);
        public Task<CommonResponse> SignOut(Guid rTokenId);
        public Task<CommonResponse<TokensDTO>> SignUp(RegistrationInfoDTO registrationInfoDTO);
        public Task<CommonResponse<TokensDTO>> Refresh(Guid rTokenId);
    }
}
