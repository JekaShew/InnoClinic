using AuthorizationAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories
{
    public interface IRoleRepository
    {
        public void CreateRole(Role role);
        public Task<IEnumerable<Role>> GetAllRolesAsync(bool trackChanges);
        public void UpdateRole(Role role);
        public void DeleteRole(Role role);
        public Task<IEnumerable<Role>> GetRolesWithExpressionAsync(Expression<Func<Role, bool>> expression, bool trackChanges);
    }
}
