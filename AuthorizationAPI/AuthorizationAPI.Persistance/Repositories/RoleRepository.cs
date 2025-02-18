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

    public async Task<IEnumerable<Role>> GetAllRolesAsync(bool trackChanges)
    {
        return trackChanges ?
            await _authDBContext.Roles.ToListAsync() :
            await _authDBContext.Roles.AsNoTracking().ToListAsync();
    }

    public async Task<Role> GetRoleByIdAsync(Guid roleId, bool trackChanges)
    {
        return trackChanges ?
            await _authDBContext.Roles.FirstOrDefaultAsync(us => us.Id.Equals(roleId)) :
            await _authDBContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id.Equals(roleId));
    }

    public async Task<IEnumerable<Role>> GetRolesWithExpressionAsync(Expression<Func<Role, bool>> expression, bool trackChanges)
    {
        return trackChanges ?
            await _authDBContext.Roles.Where(expression).ToListAsync() :
            await _authDBContext.Roles.AsNoTracking().Where(expression).ToListAsync();
    }
     
    public async Task CreateRoleAsync(Role role)
    {
        await _authDBContext.Roles.AddAsync(role);
    }

    public void DeleteRole(Role role)
    {
        _authDBContext.Roles.Remove(role);
    }

    public async Task UpdateRoleAsync(Role updatedRole)
    {
        var role = await _authDBContext.Roles.FindAsync(updatedRole.Id);
        _authDBContext.Roles.Entry(role).State = EntityState.Detached;
        _authDBContext.Roles.Update(updatedRole);
    }
}
