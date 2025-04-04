using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories;

public class UserStatusRepository : IUserStatusRepository
{
    private readonly AuthDBContext _authDBContext;

    public UserStatusRepository(AuthDBContext authDBContext) 
    {
        _authDBContext = authDBContext;
    }

    public async Task<IEnumerable<UserStatus>> GetAllUserStatusesAsync()
    {
        return await _authDBContext.UserStatuses.AsNoTracking().ToListAsync();
    }

    public async Task<UserStatus?> GetUserStatusByIdAsync(Guid userStatusId)
    {
        return await _authDBContext.UserStatuses
                .FirstOrDefaultAsync(us => us.Id.Equals(userStatusId));
    }

    public async Task<IEnumerable<UserStatus>> GetUserStatusesWithExpressionAsync(Expression<Func<UserStatus, bool>> expression)
    {
        return await _authDBContext.UserStatuses
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
        if(userStatus is not null)
        {
            _authDBContext.UserStatuses.Entry(userStatus).State = EntityState.Detached;
        }

        _authDBContext.UserStatuses.Update(updatedUserStatus);
    }
}
