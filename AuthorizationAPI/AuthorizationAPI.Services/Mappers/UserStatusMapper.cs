using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;
using Riok.Mapperly.Abstractions;

namespace AuthorizationAPI.Services.Mappers
{
    [Mapper]
    public static partial class UserStatusMapper
    {
        public static partial UserStatusInfoDTO? UserStatusToUserStatusInfoDTO(UserStatus? userStatus);
        public static partial UserStatus? UserStatusForCreateDTOToUserStatus(UserStatusForCreateDTO? userStatusForCreateDTO);
        public static partial UserStatus? UserStatusForUpdateDTOToUserStatus(UserStatusForUpdateDTO? userStatusForupdateDTO);

    }
}
