using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public async Task<Guid> CreateUserAsync(User user)
    {
        await _authDBContext.Users.AddAsync(user);

        return user.Id;
    }

    public void DeleteUser(User user)
    {
        _authDBContext.Users.Remove(user);
    }

    public async Task<User> UpdateUserAsync(User updatedUser)
    {
        var user = await _authDBContext.Users.FindAsync(updatedUser.Id);
        if (user is not null)
        {
            _authDBContext.Users.Entry(user).State = EntityState.Detached;
        }
        
        _authDBContext.Users.Update(updatedUser);

        return updatedUser;
    }
}

