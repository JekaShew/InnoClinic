using AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces
{
    public interface IRefreshTokenService
    {
        public Task<CommonResponse<IEnumerable<UserLoggedInInfoDTO>>> TakeAllLoggedInUsers();
        public Task<CommonResponse<RefreshTokenInfoDTO>> TakeRefreshTokenInfoByRefreshTokenId(Guid refreshTokenId);
        public Task<CommonResponse> DeleteRefreshTokenByRTokenId(Guid refreshTokenId);
        public Task<CommonResponse> RevokeRefreshTokenByRefreshTokenId(Guid rTokenId);
    }
}
