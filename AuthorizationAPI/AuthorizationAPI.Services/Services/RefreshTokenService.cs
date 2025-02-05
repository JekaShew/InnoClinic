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

        public async Task<ResponseMessage> DeleteRefreshTokenByRTokenId(Guid refreshTokenId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new ResponseMessage(MessageConstants.ForbiddenMessage, false);

            var refreshToken = (await _repositoryManager.RefreshToken
                    .GetRefreshTokensWithExpressionAsync(rt => rt.Id.Equals(refreshTokenId), true))
                    .FirstOrDefault();
            if (refreshToken is null)
                return new ResponseMessage(MessageConstants.NotFoundMessage, false);

            _repositoryManager.RefreshToken.DeleteRefreshToken(refreshToken);
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
        }

        public async Task<ResponseMessage> RevokeRefreshTokenByRefreshTokenId(Guid rTokenId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new ResponseMessage(MessageConstants.ForbiddenMessage, false);

            var refreshToken = (await _repositoryManager.RefreshToken
                    .GetRefreshTokensWithExpressionAsync(rt => rt.Id.Equals(rTokenId), true))
                    .FirstOrDefault(); 
            if(refreshToken is null)
                return new ResponseMessage(MessageConstants.NotFoundMessage, false);

            refreshToken.IsRevoked = true;
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessMessage, true);
        }

        public async Task<ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>> GetAllLoggedInUsers()
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>(MessageConstants.ForbiddenMessage, false);

            var refreshTokenCollection = await _repositoryManager.RefreshToken
                    .GetAllRefreshTokensAsync(false);
            if(!refreshTokenCollection.Any())
                return new ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>(MessageConstants.NotFoundMessage, false);

            var userLoggedInInfoDTOs = refreshTokenCollection.Select(rt => RefreshTokenMapper.RefreshTokenToUserLoggedInInfoDTO(rt));

            return new ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>(MessageConstants.SuccessMessage, true, userLoggedInInfoDTOs);
        }

        public async Task<ResponseMessage<RefreshTokenInfoDTO>> GetRefreshTokenInfoByRefreshTokenId(Guid refreshTokenId)
        {
            var userAdmin = await _userService.IsCurrentUserAdministrator();
            if (userAdmin is null)
                return new ResponseMessage<RefreshTokenInfoDTO>(MessageConstants.ForbiddenMessage, false);

            var refreshToken = (await _repositoryManager.RefreshToken
                    .GetRefreshTokensWithExpressionAsync(rt => rt.Id.Equals(refreshTokenId), false))
                    .FirstOrDefault();
            if (refreshToken is null)
                return new ResponseMessage<RefreshTokenInfoDTO>(MessageConstants.NotFoundMessage, false);

            var refreshTokenInfoDTO = RefreshTokenMapper.RefreshTokenToRefreshTokenInfoDTO(refreshToken);

            return new ResponseMessage<RefreshTokenInfoDTO>(MessageConstants.SuccessMessage, true, refreshTokenInfoDTO);
        }
    }
}
