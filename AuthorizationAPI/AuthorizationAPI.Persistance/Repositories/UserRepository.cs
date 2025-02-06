using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AuthDBContext authDBContext) : base(authDBContext) 
    { }

    public async Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges)
    {
        return await GetAll(trackChanges).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetUsersWithExpressionAsync(Expression<Func<User, bool>> expression, bool trackChanges)
    {
        return await GetWithExpression(expression, trackChanges).ToListAsync();
    }

    public void CreateUser(User user)
    {
        Create(user);
    }

    public void DeleteUser(User user)
    {
        Delete(user);
    }

    public void UpdateUser(User updatedUser)
    {
        Update(updatedUser);
    }
}

