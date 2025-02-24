using AuthorizationAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories;

public interface IRoleRepository
{
    public  Task CreateRoleAsync(Role role);
    public Task<IEnumerable<Role>> GetAllRolesAsync();
    public Task<Role?> GetRoleByIdAsync(Guid roleID);
    public Task UpdateRoleAsync(Role role);
    public void DeleteRole(Role role);
    public Task<IEnumerable<Role>> GetRolesWithExpressionAsync(Expression<Func<Role, bool>> expression);
}
