using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUserService _userService;
        public RefreshTokenService(IRepositoryManager repositoryManager, IUserService userService)
        {
            _repositoryManager = repositoryManager;
            _userService = userService;
        }

        public async Task<CommonResponse> DeleteRefreshTokenByRTokenId(Guid refreshTokenId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse(false, MessageConstants.ForbiddenMessage);

            var refreshToken = (await _repositoryManager.RefreshToken
                    .GetRefreshTokensWithExpressionAsync(rt => rt.Id.Equals(refreshTokenId), true))
                    .FirstOrDefault();
            if (refreshToken is null)
                return new CommonResponse(false, MessageConstants.NotFoundMessage);

            _repositoryManager.RefreshToken.DeleteRefreshToken(refreshToken);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessDeleteMessage);
        }

        public async Task<CommonResponse> RevokeRefreshTokenByRefreshTokenId(Guid rTokenId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse(false, MessageConstants.ForbiddenMessage);

            var refreshToken = (await _repositoryManager.RefreshToken
                    .GetRefreshTokensWithExpressionAsync(rt => rt.Id.Equals(rTokenId), true))
                    .FirstOrDefault(); 
            if(refreshToken is null)
                return new CommonResponse(false, MessageConstants.NotFoundMessage);

            refreshToken.IsRevoked = true;
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessMessage);
        }

        public async Task<CommonResponse<IEnumerable<UserLoggedInInfoDTO>>> TakeAllLoggedInUsers()
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse<IEnumerable<UserLoggedInInfoDTO>>(false, MessageConstants.ForbiddenMessage);

            var refreshTokenCollection = await _repositoryManager.RefreshToken
                    .GetAllRefreshTokensAsync(false);
            if(!refreshTokenCollection.Any())
                return new CommonResponse<IEnumerable<UserLoggedInInfoDTO>>(false, MessageConstants.NotFoundMessage);

            var userLoggedInInfoDTOs = refreshTokenCollection.Select(rt => RefreshTokenMapper.RefreshTokenToUserLoggedInInfoDTO(rt));

            return new CommonResponse<IEnumerable<UserLoggedInInfoDTO>>(true, MessageConstants.SuccessMessage, userLoggedInInfoDTOs);
        }

        public async Task<CommonResponse<RefreshTokenInfoDTO>> TakeRefreshTokenInfoByRefreshTokenId(Guid refreshTokenId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new CommonResponse<RefreshTokenInfoDTO>(false, MessageConstants.ForbiddenMessage);

            var refreshToken = (await _repositoryManager.RefreshToken
                    .GetRefreshTokensWithExpressionAsync(rt => rt.Id.Equals(refreshTokenId), false))
                    .FirstOrDefault();
            if (refreshToken is null)
                return new CommonResponse<RefreshTokenInfoDTO>(false, MessageConstants.NotFoundMessage);

            var refreshTokenInfoDTO = RefreshTokenMapper.RefreshTokenToRefreshTokenInfoDTO(refreshToken);

            return new CommonResponse<RefreshTokenInfoDTO>(true, MessageConstants.SuccessMessage, refreshTokenInfoDTO);
        }
    }
}
