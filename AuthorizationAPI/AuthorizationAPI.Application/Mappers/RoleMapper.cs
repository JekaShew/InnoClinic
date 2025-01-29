using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Domain.Data.Models;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAPI.Application.Mappers
{
    [Mapper]
    public static partial class RoleMapper
    {
        public static partial Role? RoleDTOToRole(RoleDTO? roleDTO);
        public static partial RoleDTO? RoleToRoleDTO(Role? role);
    }
}
