using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Shared.DTOs;
using Riok.Mapperly.Abstractions;

namespace AuthorizationAPI.Services.Mappers
{
    [Mapper]
    public static partial class UserStatusMapper
    {
        public static partial UserStatus? UserStatusDTOToUserStatus(UserStatusDTO? userStatusDTO);
        public static partial UserStatusDTO? UserStatusToUserStatusDTO(UserStatus? userStatus);
    }
}
