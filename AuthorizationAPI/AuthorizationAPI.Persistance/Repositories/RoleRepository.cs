using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories;
  
public class RoleRepository : IRoleRepository 
{
    private readonly AuthDBContext _authDBContext;

    public RoleRepository(AuthDBContext authDBContext) 
    {
        _authDBContext = authDBContext;
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _authDBContext.Roles.AsNoTracking().ToListAsync();
    }

    public async Task<Role?> GetRoleByIdAsync(Guid roleId)
    {
        return await _authDBContext.Roles.FirstOrDefaultAsync(us => us.Id.Equals(roleId));
    }

    public async Task<IEnumerable<Role>> GetRolesWithExpressionAsync(Expression<Func<Role, bool>> expression)
    {
        return await _authDBContext.Roles.AsNoTracking().Where(expression).ToListAsync();
    }
     
    public async Task<Guid> CreateRoleAsync(Role role)
    {
        await _authDBContext.Roles.AddAsync(role);
        return role.Id;
    }

    public void DeleteRole(Role role)
    {
        _authDBContext.Roles.Remove(role);
    }

    public async Task<Role> UpdateRoleAsync(Role updatedRole)
    {
        var role = await _authDBContext.Roles.FindAsync(updatedRole.Id);
        if(role is not null)
        {
            _authDBContext.Roles.Entry(role).State = EntityState.Detached;
        }
        
        _authDBContext.Roles.Update(updatedRole);

        return updatedRole;
    }
}
