using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Shared.DTOs;
using Riok.Mapperly.Abstractions;

namespace AuthorizationAPI.Services.Mappers
{
    [Mapper]
    public static partial class RoleMapper
    {
        public static partial Role? RoleDTOToRole(RoleDTO? roleDTO);
        public static partial RoleDTO? RoleToRoleDTO(Role? role);
    }
}
