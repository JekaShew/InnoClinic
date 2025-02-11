using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;
using Riok.Mapperly.Abstractions;


namespace AuthorizationAPI.Services.Mappers;

[Mapper]
[UseStaticMapper(typeof(UserMapper))]
public static partial class RefreshTokenMapper
{
    [MapProperty(nameof(RefreshToken.User), nameof(UserLoggedInInfoDTO.UserInfo))]
    public static partial UserLoggedInInfoDTO? RefreshTokenToUserLoggedInInfoDTO(RefreshToken? refreshToken);
    public static partial RefreshTokenInfoDTO? RefreshTokenToRefreshTokenInfoDTO(RefreshToken? refreshToken);
}
