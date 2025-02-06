using AuthorizationAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories;

public interface IRefreshTokenRepository
{
    public void CreateRefreshToken(RefreshToken refreshToken);
    public Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync(bool trackChanges);
    public void UpdateRefreshToken(RefreshToken refreshToken);
    public void DeleteRefreshToken(RefreshToken refreshToken);
    public Task<IEnumerable<RefreshToken>> GetRefreshTokensWithExpressionAsync(Expression<Func<RefreshToken, bool>> expression, bool trackChanges);
}
