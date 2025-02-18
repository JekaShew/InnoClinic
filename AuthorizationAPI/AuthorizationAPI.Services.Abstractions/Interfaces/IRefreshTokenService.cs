using AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces;

public interface IRefreshTokenService
{
    public Task<ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>> GetAllLoggedInUsers();
    public Task<ResponseMessage<RefreshTokenInfoDTO>> GetRefreshTokenInfoByRefreshTokenId(Guid refreshTokenId);
    public Task<ResponseMessage> DeleteRefreshTokenByRTokenId(Guid refreshTokenId);
    public Task<ResponseMessage> RevokeRefreshTokenByRefreshTokenId(Guid refreshTokenId);
}
