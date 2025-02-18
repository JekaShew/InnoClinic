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
    public static partial UserForUpdateByAdministratorDTO? UserToUserForUpdateByAdministratorDTO(User? user);
    public static partial void UpdateUserFromUserForUpdateDTO(UserForUpdateDTO? dto, User? model);
    public static partial void UpdateUserFromUserForUpdateByAdministratorDTO(UserForUpdateByAdministratorDTO? dto, User? model);
}
