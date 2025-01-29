using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;

namespace AuthorizationAPI.Application.Interfaces
{
    public interface IRefreshToken
    {
        public Task<CustomResponse> AddRefreshToken(RefreshTokenDTO refreshTokenDTO);
        public Task<List<RefreshTokenDTO>> TakeAllRefreshTokens();
        public Task<RefreshTokenDTO> TakeRefreshTokenByRTokenId(Guid refreshTokenId);
        public Task<CustomResponse> UpdateRefreshToken(RefreshTokenDTO refreshTokenDTO);
        public Task<CustomResponse> DeleteRefreshTokenByRTokenId(Guid refreshTokenId);
    }
}
