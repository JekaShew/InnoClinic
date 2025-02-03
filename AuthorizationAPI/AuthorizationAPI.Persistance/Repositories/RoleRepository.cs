using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AuthDBContext authDBContext) : base(authDBContext) 
        { }

        public async Task<IEnumerable<Role>> GetAllRolesAsync(bool trackChanges)
        {
            return await GetAll(trackChanges).ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetRolesWithExpressionAsync(Expression<Func<Role, bool>> expression, bool trackChanges)
        {
            return await GetWithExpression(expression, trackChanges).ToListAsync();
        }

        public void CreateRole(Role role)
        {
            Create(role);
        }

        public void DeleteRole(Role role)
        {
            Delete(role);
        }

        public void UpdateRole(Role updatedRole)
        {
            Update(updatedRole);
        }
    }
}
