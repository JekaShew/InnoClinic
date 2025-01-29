using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Domain.Data.Models;
using Riok.Mapperly.Abstractions;

namespace AuthorizationAPI.Application.Mappers
{
    [Mapper]
    public static partial class UserMapper      
    {
        [MapProperty([nameof(User.UserStatus), nameof(User.UserStatus.Id)],
            [nameof(UserDTO.UserStatus), nameof(UserDTO.UserStatus.Id)])]
        [MapProperty([nameof(User.UserStatus), nameof(User.UserStatus.Title)],
            [nameof(UserDTO.UserStatus), nameof(UserDTO.UserStatus.Text)])]

        [MapProperty([nameof(User.Role), nameof(User.Role.Id)],
            [nameof(UserDTO.Role), nameof(UserDTO.Role.Id)])]
        [MapProperty([nameof(User.Role), nameof(User.Role.Title)],
            [nameof(UserDTO.Role), nameof(UserDTO.Role.Text)])]
        public static partial UserDTO? UserToUserDTO(User? user);
        public static partial AuthorizationInfoDTO? UserToAuthorizationInfoDTO(User? user);
        public static partial User? UserDTOToUser(UserDTO? userDTO);
        public static partial UserDetailedDTO? RegistrationInfoDTOToUserDetailedDTO(RegistrationInfoDTO registrationInfoDTO);
        public static partial User? UserDetailedDTOToUser(UserDetailedDTO? userDTO);  
    }
}