using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using InnoClinic.CommonLibrary.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AuthDBContext _authDBContext;

        public RoleRepository(AuthDBContext authDBContext)
        {
            _authDBContext = authDBContext;
        }

        public async Task<CustomResponse> AddRole(Role role)
        {
            await _authDBContext.AddAsync(role);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Added!");
        }

        public async Task<CustomResponse> DeleteRoleById(Guid roleId)
        {
            var role = await _authDBContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id.Equals(roleId));

            _authDBContext.Roles.Remove(role!);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Deleted!");
        }

        public async Task<CustomResponse<List<Role>>> TakeAllRoles()
        {
            var roles = await _authDBContext.Roles.AsNoTracking().ToListAsync();

            return new CustomResponse<List<Role>>(true,"Success!",roles);
        }

        public async Task<CustomResponse<Role>> TakeRoleById(Guid roleId)
        {
            var role = await _authDBContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id.Equals(roleId));

            return new CustomResponse<Role>(true, "Success!", role);
        }

        public async Task<CustomResponse> UpdateRole(Role updatedRole)
        {
            var role = await _authDBContext.Roles.FindAsync(updatedRole.Id);

            _authDBContext.Roles.Entry(role).State = EntityState.Detached;
            _authDBContext.Roles.Update(updatedRole);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Updated!");
        }

        public async Task<CustomResponse<Role>> TakeRoleWithPredicate(Expression<Func<Role, bool>> predicate)
        {
            var role = await _authDBContext.Roles
                    .Where(predicate)
                    .FirstOrDefaultAsync();

            return new CustomResponse<Role>(true,"Success!", role);
        }
    }
}
