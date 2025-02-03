using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AuthDBContext authDBContext) : base(authDBContext) 
        { }

        public async Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync(bool trackChanges)
        {
            return await GetAll(trackChanges).ToListAsync();
        }

        public async Task<IEnumerable<RefreshToken>> GetRefreshTokensWithExpressionAsync(Expression<Func<RefreshToken, bool>> expression, bool trackChanges)
        {
            return await GetWithExpression(expression,trackChanges).ToListAsync();
        }

        public void CreateRefreshToken(RefreshToken refreshToken)
        {
            Create(refreshToken);
        }

        public void DeleteRefreshToken(RefreshToken refreshToken)
        {
            Delete(refreshToken);
        }

        public void UpdateRefreshToken(RefreshToken updatedRefreshToken)
        {
            Update(updatedRefreshToken);
        }
    }
}

