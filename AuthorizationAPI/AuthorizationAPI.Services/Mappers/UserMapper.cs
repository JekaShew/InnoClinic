using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using Riok.Mapperly.Abstractions;

namespace AuthorizationAPI.Services.Mappers;

[Mapper]
public static partial class UserMapper
{
    public static partial UserDetailedDTO? UserToUserDetailedDTO(User? user);
    public static partial User? RegistrationInfoDTOToUser(RegistrationInfoDTO? registrationInfoDTO);
    public static partial UserInfoDTO? UserToUserInfoDTO(User? user);
    public static partial User? UserInfoDTOToUser(UserInfoDTO? userInfoDTO);
    public static partial User? UserForUpdateDTOToUser(UserForUpdateDTO? userForUpdateDTO);
}
