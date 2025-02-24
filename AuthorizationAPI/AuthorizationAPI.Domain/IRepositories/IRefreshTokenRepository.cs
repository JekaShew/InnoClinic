using AuthorizationAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories;

public interface IRefreshTokenRepository
{
    public Task CreateRefreshTokenAsync(RefreshToken refreshToken);
    public Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync();
    public Task<IEnumerable<RefreshToken>> GetAllExpiredRefreshTokensAsync();
    public Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId);
    public Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    public void DeleteRefreshToken(RefreshToken refreshToken);
    public Task<IEnumerable<RefreshToken>> GetRefreshTokensWithExpressionAsync(Expression<Func<RefreshToken, bool>> expression);
}
