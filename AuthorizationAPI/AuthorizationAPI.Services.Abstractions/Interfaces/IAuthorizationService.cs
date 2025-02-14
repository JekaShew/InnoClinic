using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces;

public interface IAuthorizationService
{
    public Task<ResponseMessage<TokensDTO>> SignIn(LoginInfoDTO loginInfoDTO);
    public Task<ResponseMessage> SignOut(Guid rTokenId);
    public Task<ResponseMessage> SignUp(RegistrationInfoDTO registrationInfoDTO);
    public Task<ResponseMessage<TokensDTO>> Refresh(Guid rTokenId);
    public string GenerateEmailConfirmationTokenByEmailAndDateTime(string email, string dateTimeString);
}
