using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories;

public class UserStatusRepository : /*BaseRepository<UserStatus>,*/ IUserStatusRepository
{
    private readonly AuthDBContext _authDBContext;

    public UserStatusRepository(AuthDBContext authDBContext) /*: base(authDBContext)*/ 
    {
        _authDBContext = authDBContext;
    }

    public async Task<IEnumerable<UserStatus>> GetAllUserStatusesAsync(bool trackChanges)
    {
        return trackChanges ? 
            await _authDBContext.UserStatuses.ToListAsync() : 
            await _authDBContext.UserStatuses.AsNoTracking().ToListAsync();
    }

    public async Task<UserStatus> GetUserStatusByIdAsync(Guid userStatusId, bool trackChanges)
    {
        return trackChanges ?
            await _authDBContext.UserStatuses
                .FirstOrDefaultAsync(us => us.Id.Equals(userStatusId)):
            await _authDBContext.UserStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(us => us.Id.Equals(userStatusId));
    }

    public async Task<IEnumerable<UserStatus>> GetUserStatusesWithExpressionAsync(Expression<Func<UserStatus, bool>> expression, bool trackChanges)
    {
        return trackChanges ?
            await _authDBContext.UserStatuses
                .Where(expression)
                .ToListAsync() :
            await _authDBContext.UserStatuses
                .AsNoTracking()
                .Where(expression)
                .ToListAsync();
    }

    public async Task CreateUserStatusAsync(UserStatus userStatus)
    {
        await _authDBContext.UserStatuses.AddAsync(userStatus);
    }

    public void DeleteUserStatus(UserStatus userStatus)
    {
        _authDBContext.UserStatuses.Remove(userStatus);
    }

    public async Task UpdateUserStatusAsync(UserStatus updatedUserStatus)
    {
        var userStatus = await _authDBContext.UserStatuses.FindAsync(updatedUserStatus.Id);
        _authDBContext.UserStatuses.Entry(userStatus).State = EntityState.Detached;
        _authDBContext.UserStatuses.Update(updatedUserStatus);
    }
}
