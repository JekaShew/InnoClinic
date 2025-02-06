using AuthorizationAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories;

public interface IUserRepository
{
    public void CreateUser(User user);
    public Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges);
    public void UpdateUser(User user);
    public void DeleteUser(User user);
    public Task<IEnumerable<User>> GetUsersWithExpressionAsync(Expression<Func<User, bool>> expression, bool trackChanges);
}
