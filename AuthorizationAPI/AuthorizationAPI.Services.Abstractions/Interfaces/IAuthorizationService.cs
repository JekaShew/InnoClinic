using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces;

public interface IAuthorizationService
{
    public Task<ResponseMessage<TokensDTO>> SignIn(LoginInfoDTO loginInfoDTO);
    public Task<ResponseMessage> SignOut(Guid rTokenId);
    public Task<ResponseMessage<Guid>> SignUp(RegistrationInfoDTO registrationInfoDTO);
    public Task<ResponseMessage<TokensDTO>> Refresh(Guid rTokenId);
    public Task<ResponseMessage> ResendEmailVerification(LoginInfoDTO loginInfoDTO);
}
