using AuthorizationAPI.Domain.Data.Models;
using InnoClinic.CommonLibrary.Response;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories
{
    public interface IRoleRepository
    {
        public Task<CustomResponse> AddRole(Role role);
        public Task<CustomResponse<List<Role>>> TakeAllRoles();
        public Task<CustomResponse<Role>> TakeRoleById(Guid roleId);
        public Task<CustomResponse> UpdateRole(Role role);
        public Task<CustomResponse> DeleteRoleById(Guid roleId);
        public Task<CustomResponse<Role>> TakeRoleWithPredicate(Expression<Func<Role, bool>> predicate);
    }
}
