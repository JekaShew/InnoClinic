using AuthorizationAPI.Domain.Data.Models;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Domain.IRepositories
{
    public interface IRefreshTokenRepository
    {
        public Task<CustomResponse> AddRefreshToken(RefreshToken refreshToken);
        public Task<CustomResponse<List<RefreshToken>>> TakeAllRefreshTokens();
        public Task<CustomResponse<RefreshToken>> TakeRefreshTokenByRTokenId(Guid refreshTokenId);
        public Task<CustomResponse> UpdateRefreshToken(RefreshToken refreshToken);
        public Task<CustomResponse> DeleteRefreshTokenByRTokenId(Guid refreshTokenId);
    }
}
