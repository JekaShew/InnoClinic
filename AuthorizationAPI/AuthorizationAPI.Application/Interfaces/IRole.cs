using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Domain.Data.Models;
using InnoShop.CommonLibrary.Response;
using System.Linq.Expressions;

namespace AuthorizationAPI.Application.Interfaces
{
    public interface IRole
    {
        public Task<CustomResponse> AddRole(RoleDTO roleDTO);
        public Task<List<RoleDTO>> TakeAllRoles();
        public Task<RoleDTO> TakeRoleById(Guid roleId);
        public Task<CustomResponse> UpdateRole(RoleDTO roleDTO);
        public Task<CustomResponse> DeleteRoleById(Guid roleId);
        public Task<RoleDTO> TakeRoleDTOWithPredicate(Expression<Func<Role, bool>> predicate);
    }
}
