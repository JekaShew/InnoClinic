using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using AuthorizationAPI.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace AuthorizationAPI.Persistance.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDBContext _authDBContext;

    public UserRepository(AuthDBContext authDBContext)
    { 
        _authDBContext = authDBContext;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _authDBContext.Users
                .Include(r => r.Role)
                .Include(us => us.UserStatus)
                .AsNoTracking()
                .ToListAsync();
    }

    public async  Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _authDBContext.Users
                .Include(r => r.Role)
                .Include(us => us.UserStatus)
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _authDBContext.Users
               .Include(r => r.Role)
               .Include(us => us.UserStatus)
               .FirstOrDefaultAsync(u => u.Email.Equals(email));
    }

    public async Task<IEnumerable<User>> GetUsersWithExpressionAsync(Expression<Func<User, bool>> expression)
    {
        return await _authDBContext.Users
                .Include(r => r.Role)
                .Include(us => us.UserStatus)
                .AsNoTracking()
                .Where(expression)
                .ToListAsync();
    }

    public async Task CreateUserAsync(User user)
    {
        await _authDBContext.Users.AddAsync(user);
    }

    public void DeleteUser(User user)
    {
        _authDBContext.Users.Remove(user);
    }

    public async Task UpdateUserAsync(User updatedUser)
    {
        var user = await _authDBContext.Users.FindAsync(updatedUser.Id);
        if (user is not null)
        {
            _authDBContext.Users.Entry(user).State = EntityState.Detached;
        }
        
        _authDBContext.Users.Update(updatedUser);
    }

    //public async Task<bool> IsCurrentUserAdministrator(Guid currentUserId)
    //{
    //    return await _authDBContext.Users
    //        .AsNoTracking()
    //        .AnyAsync(u => 
    //            u.Id.Equals(currentUserId) &&
    //            u.RoleId.Equals(DBConstants.AdministratorRoleId));
    //}

    //public async Task<bool> IsEmailRegistered(string email)
    //{
    //    return await _authDBContext.Users
    //            .AsNoTracking()
    //            .AnyAsync(u => u.Email.Equals(email));
    //}
}

