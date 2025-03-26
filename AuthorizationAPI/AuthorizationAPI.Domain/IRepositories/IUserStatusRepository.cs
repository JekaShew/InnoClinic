using AuthorizationAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories;

public interface IUserStatusRepository
{
    public Task<Guid> CreateUserStatusAsync(UserStatus userStatus);
    public Task<IEnumerable<UserStatus>> GetAllUserStatusesAsync();
    public Task<UserStatus?> GetUserStatusByIdAsync(Guid userStatusId);
    public Task<UserStatus> UpdateUserStatusAsync(UserStatus userStatus);
    public void DeleteUserStatus(UserStatus userStatus);
    public Task<IEnumerable<UserStatus>> GetUserStatusesWithExpressionAsync(Expression<Func<UserStatus, bool>> expression);
}
