using AuthorizationAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories;

public interface IUserStatusRepository
{
    public void CreateUserStatus(UserStatus userStatus);
    public Task<IEnumerable<UserStatus>> GetAllUserStatusesAsync(bool trackChanges);
    public void UpdateUserStatus(UserStatus userStatus);
    public void DeleteUserStatus(UserStatus userStatus);
    public Task<IEnumerable<UserStatus>> GetUserStatusesWithExpressionAsync(Expression<Func<UserStatus, bool>> expression, bool trackChanges);
}
