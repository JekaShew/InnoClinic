using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories;

public class UserStatusRepository : BaseRepository<UserStatus>, IUserStatusRepository
{
    public UserStatusRepository(AuthDBContext authDBContext) : base(authDBContext) 
    {
    }

    public async Task<IEnumerable<UserStatus>> GetAllUserStatusesAsync(bool trackChanges)
    {
        return await GetAll(trackChanges).ToListAsync();
    }

    public async Task<IEnumerable<UserStatus>> GetUserStatusesWithExpressionAsync(Expression<Func<UserStatus, bool>> expression, bool trackChanges)
    {
        return await GetWithExpression(expression, trackChanges).ToListAsync();
    }

    public void CreateUserStatus(UserStatus userStatus)
    {
        Create(userStatus);
    }

    public void DeleteUserStatus(UserStatus userStatus)
    {
        Delete(userStatus);
    }

    public void UpdateUserStatus(UserStatus updatedUserStatus)
    {
        Update(updatedUserStatus);
    }
}
