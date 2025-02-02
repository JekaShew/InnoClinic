using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Shared.DTOs;
using Riok.Mapperly.Abstractions;

namespace AuthorizationAPI.Services.Mappers
{
    [Mapper]
    public static partial class UserMapper
    {
        [MapProperty([nameof(User.UserStatus), nameof(User.UserStatus.Id)],
            [nameof(UserDetailedDTO.UserStatus), nameof(UserDetailedDTO.UserStatus.Id)])]
        [MapProperty([nameof(User.UserStatus), nameof(User.UserStatus.Title)],
            [nameof(UserDetailedDTO.UserStatus), nameof(UserDetailedDTO.UserStatus.Text)])]

        [MapProperty([nameof(User.Role), nameof(User.Role.Id)],
            [nameof(UserDetailedDTO.Role), nameof(UserDetailedDTO.Role.Id)])]
        [MapProperty([nameof(User.Role), nameof(User.Role.Title)],
            [nameof(UserDetailedDTO.Role), nameof(UserDetailedDTO.Role.Text)])]
        public static partial UserDetailedDTO? UserToUserDetailedDTO(User? user);

        public static partial UserInfoDTO? UserToUserInfoDTO(User? user);
        public static partial User? UserInfoDTOToUser(UserInfoDTO? userInfoDTO);
        public static partial User? RegistrationInfoDTOToUser(RegistrationInfoDTO? registrationInfoDTO);
    }
}
