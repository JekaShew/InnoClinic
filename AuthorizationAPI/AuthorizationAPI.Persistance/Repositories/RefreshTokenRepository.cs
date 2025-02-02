using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using InnoClinic.CommonLibrary.Response;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace AuthorizationAPI.Persistance.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthDBContext _authDBContext;
        public RefreshTokenRepository(AuthDBContext authDBContext)
        {
            _authDBContext = authDBContext;
        }
        public async Task<CustomResponse> AddRefreshToken(RefreshToken refreshToken)
        {

            await _authDBContext.RefreshTokens.AddAsync(refreshToken);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Added!");
        }

        public async Task<CustomResponse> DeleteRefreshTokenByRTokenId(Guid rTokenId)
        {
            var refreshToken = await _authDBContext.RefreshTokens
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id.Equals(rTokenId));

            _authDBContext.RefreshTokens.Remove(refreshToken);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Deleted!");
        }

        public async Task<CustomResponse<List<RefreshToken>>> TakeAllRefreshTokens()
        {
            var refreshTokenes = await _authDBContext.RefreshTokens
                    .Include(u => u.User)
                    .AsNoTracking()
                    .ToListAsync();

            return new CustomResponse<List<RefreshToken>>(true, "Success!", refreshTokenes);
        }

        public async Task<CustomResponse<RefreshToken>> TakeRefreshTokenByRTokenId(Guid rTokenId)
        {
            var refreshToken = await _authDBContext.RefreshTokens
                    .Include(u => u.User)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == rTokenId);

            return new CustomResponse<RefreshToken>(true, "Success!", refreshToken);

        }

        public async Task<CustomResponse> UpdateRefreshToken(RefreshToken updatedRefreshToken)
        {
            var refreshToken = await _authDBContext.RefreshTokens.FindAsync(updatedRefreshToken.Id);

            _authDBContext.RefreshTokens.Entry(refreshToken).State = EntityState.Detached;
            _authDBContext.RefreshTokens.Update(updatedRefreshToken);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Updated!");
        }
    }
}
