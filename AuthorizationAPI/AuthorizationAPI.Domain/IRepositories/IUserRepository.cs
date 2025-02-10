using AuthorizationAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories;

public interface IUserRepository
{
    public Task CreateUserAsync(User user);
    public Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges = false);
    public Task<User> GetUserByIdAsync(Guid userId, bool trackChanges = false);
    public Task<User> GetUserByEmailAsync(string email, bool trackChanges = false);
    public Task UpdateUserAsync(User user);
    public void DeleteUser(User user);
    public Task<IEnumerable<User>> GetUsersWithExpressionAsync(Expression<Func<User, bool>> expression, bool trackChanges = false);
    public Task<bool> IsCurrentUserAdministrator(Guid currentUserId);
    public Task<bool> IsEmailRegistered(string email);
}
