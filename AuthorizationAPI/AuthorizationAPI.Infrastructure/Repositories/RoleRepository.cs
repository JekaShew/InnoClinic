using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using AuthorizationAPI.Application.Mappers;
using AuthorizationAPI.Domain.Data;
using AuthorizationAPI.Domain.Data.Models;
using InnoShop.CommonLibrary.Response;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace AuthorizationAPI.Infrastructure.Repositories
{
    public class RoleRepository : IRole
    {
        private readonly AuthDBContext _authDBContext;

        public RoleRepository(AuthDBContext authDBContext)
        {
            _authDBContext = authDBContext;
        }

        public async Task<CustomResponse> AddRole(RoleDTO roleDTO)
        {
            var role = RoleMapper.RoleDTOToRole(roleDTO);
            
            await _authDBContext.AddAsync(role);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Added!");
        }

        public async Task<CustomResponse> DeleteRoleById(Guid roleId)
        {
            var role = await _authDBContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id.Equals(roleId));

            _authDBContext.Remove(role);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Deleted!");
        }

        public async Task<List<RoleDTO>> TakeAllRoles()
        {
            var roleDTOs = (await _authDBContext.Roles.AsNoTracking().ToListAsync()).Select(r => RoleMapper.RoleToRoleDTO(r)).ToList();

            return roleDTOs;
        }

        public async Task<RoleDTO> TakeRoleById(Guid roleId)
        {
            var roleDTO = RoleMapper.RoleToRoleDTO(await _authDBContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id.Equals(roleId)));

            return roleDTO;
        }

        public async Task<CustomResponse> UpdateRole(RoleDTO roleDTO)
        {
            var role = await _authDBContext.Roles.FindAsync(roleDTO.Id);

            ApplyPropertiesFromDTOToModel(roleDTO, role);

            _authDBContext.Roles.Update(role);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Updated!");
        }

        public async Task<RoleDTO> TakeRoleDTOWithPredicate(Expression<Func<Role, bool>> predicate)
        {
            var role = await _authDBContext.Roles
                    .Where(predicate)
                    .FirstOrDefaultAsync();

            if (role != null)
                return RoleMapper.RoleToRoleDTO(role);

            return null;
        }

        private void ApplyPropertiesFromDTOToModel(RoleDTO roleDTO, Role role)
        {
            var dtoProperties = roleDTO.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var modelProperties = role.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var dtoProperty in dtoProperties)
            {
                var modelProperty = modelProperties.FirstOrDefault(p => p.Name == dtoProperty.Name && p.PropertyType == dtoProperty.PropertyType);
                if (modelProperty != null)
                {
                    modelProperty.SetValue(role, dtoProperty.GetValue(roleDTO));
                }
            }
        }
    }
}
