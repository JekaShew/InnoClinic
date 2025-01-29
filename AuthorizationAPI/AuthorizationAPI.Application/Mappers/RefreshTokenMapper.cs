using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Domain.Data.Models;
using Riok.Mapperly.Abstractions;

namespace AuthorizationAPI.Application.Mappers
{
    [Mapper]
    public static partial class RefreshTokenMapper
    {
        [MapProperty([nameof(RefreshToken.User), nameof(RefreshToken.User.Id)],
            [nameof(RefreshTokenDTO.User), nameof(RefreshTokenDTO.User.Id)])]
        [MapProperty([nameof(RefreshToken.User), nameof(RefreshToken.User.FIO)],
            [nameof(RefreshTokenDTO.User), nameof(RefreshTokenDTO.User.Text)])]
        public static partial RefreshTokenDTO? RefreshTokenToRefreshTokenDTO(RefreshToken? refreshToken);
        public static partial RefreshToken? RefreshTokenDTOToRefreshToken(RefreshTokenDTO? refreshTokenDTO);

        
    }
}
