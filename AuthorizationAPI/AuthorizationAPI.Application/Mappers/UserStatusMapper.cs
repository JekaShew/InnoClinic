using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Domain.Data.Models;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAPI.Application.Mappers
{
    [Mapper]
    public static partial class UserStatusMapper
    {
        public static partial UserStatus? UserStatusDTOToUserStatus(UserStatusDTO? userStatusDTO);
        public static partial UserStatusDTO? UserStatusToUserStatusDTO(UserStatus? userStatus);
    }
}
