using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AuthDBContext _authDBContext;

    public RefreshTokenRepository(AuthDBContext authDBContext)
    {
        _authDBContext = authDBContext;
    }

    public async Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync()
    {
        return await _authDBContext.RefreshTokens
                .Include(u => u.User)
                    .ThenInclude(r => r.Role)
                .Include(u =>  u.User)
                    .ThenInclude(us => us.UserStatus)
                .AsNoTracking()
                .ToListAsync();
    }

    public async Task<IEnumerable<RefreshToken>> GetAllExpiredRefreshTokensAsync()
    {
        return await _authDBContext.RefreshTokens
                .Where(rt =>
                rt.ExpireDate.Equals(DateTime.MinValue)
                ||
                rt.ExpireDate <= DateTime.UtcNow)
                .AsNoTracking()
                .ToListAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId)
    {
        return await _authDBContext.RefreshTokens
                    .Include(u => u.User)
                        .ThenInclude(r => r.Role)
                    .Include(u => u.User)
                        .ThenInclude(us => us.UserStatus)
                    .FirstOrDefaultAsync(rt => rt.Id.Equals(refreshTokenId)); 
    }

    public async Task<IEnumerable<RefreshToken>> GetRefreshTokensWithExpressionAsync(Expression<Func<RefreshToken, bool>> expression)
    {
        return await _authDBContext.RefreshTokens
                .Include(u => u.User)
                    .ThenInclude(r => r.Role)
                .Include(u => u.User)
                    .ThenInclude(us => us.UserStatus)
                .AsNoTracking()
                .Where(expression)
                .ToListAsync();
    }

    public async Task CreateRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _authDBContext.RefreshTokens.AddAsync(refreshToken);
    }

    public void DeleteRefreshToken(RefreshToken refreshToken)
    {
        _authDBContext.RefreshTokens.Remove(refreshToken);
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken updatedRefreshToken)
    {
        var refreshToken = await _authDBContext.RefreshTokens.FindAsync(updatedRefreshToken.Id);
        if(refreshToken is not null)
        {
            _authDBContext.RefreshTokens.Entry(refreshToken).State = EntityState.Detached;
        }
        
        _authDBContext.RefreshTokens.Update(updatedRefreshToken);
    }
}

