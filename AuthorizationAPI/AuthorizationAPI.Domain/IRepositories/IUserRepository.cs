using AuthorizationAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories;

public interface IUserRepository
{
    public Task<Guid> CreateUserAsync(User user);
    public Task<IEnumerable<User>> GetAllUsersAsync();
    public Task<User?> GetUserByIdAsync(Guid userId);
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<User> UpdateUserAsync(User user);
    public void DeleteUser(User user);
    public Task<IEnumerable<User>> GetUsersWithExpressionAsync(Expression<Func<User, bool>> expression);
}
