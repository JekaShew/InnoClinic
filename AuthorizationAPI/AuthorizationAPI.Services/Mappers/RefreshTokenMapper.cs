using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;
using Riok.Mapperly.Abstractions;

namespace AuthorizationAPI.Services.Mappers
{
    [Mapper]
    public static partial class RefreshTokenMapper
    {
        public static partial UserLoggedInInfoDTO? RefreshTokenToUserLoggedInInfoDTO(RefreshToken? refreshToken);
        public static partial RefreshTokenInfoDTO? RefreshTokenToRefreshTokenInfoDTO(RefreshToken? refreshToken);
    }
}
