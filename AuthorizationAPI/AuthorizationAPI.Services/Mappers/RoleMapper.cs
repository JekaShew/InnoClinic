using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using Riok.Mapperly.Abstractions;

namespace AuthorizationAPI.Services.Mappers
{
    [Mapper]
    public static partial class RoleMapper
    {
        public static partial Role? RoleInfoDTOToRole(RoleInfoDTO? roleDTO);
        public static partial RoleInfoDTO? RoleToRoleInfoDTO(Role? role);
        public static partial Role? RoleForCreateDTOToRole(RoleForCreateDTO? roleForCreateDTO);
        public static partial Role? RoleForUpdateDTOToRole(RoleForUpdateDTO? roleForUpdateDTO);
    }
}
