using AuthorizationAPI.Shared.DTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Abstractions.Interfaces
{
    public interface IRefreshTokenService
    {
        public Task<CommonResponse> AddRefreshToken(RefreshTokenDTO refreshTokenDTO);
        public Task<List<RefreshTokenDTO>> TakeAllRefreshTokens();
        public Task<RefreshTokenDTO> TakeRefreshTokenByRefreshTokenId(Guid refreshTokenId);
        //public Task<CustomResponse> UpdateRefreshToken(RefreshTokenDTO refreshTokenDTO);
        public Task<CommonResponse> DeleteRefreshTokenByRTokenId(Guid refreshTokenId);
    }
}
