using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;
using CommonLibrary.CommonService;
using InnoClinic.CommonLibrary.Response;

namespace AuthorizationAPI.Services.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ICommonService _commonService;
    public RefreshTokenService(
            IRepositoryManager repositoryManager,
            ICommonService commonService)
    {
        _repositoryManager = repositoryManager;
        _commonService = commonService;
    }

    public async Task<ResponseMessage> DeleteRefreshTokenByRTokenId(Guid refreshTokenId)
    {
        var currentUserId = _commonService.GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        var refreshToken = await _repositoryManager.RefreshToken.GetRefreshTokenByIdAsync(refreshTokenId);
        if (refreshToken is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        _repositoryManager.RefreshToken.DeleteRefreshToken(refreshToken);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
    }

    public async Task<ResponseMessage> RevokeRefreshTokenByRefreshTokenId(Guid refreshTokenId)
    {
        var currentUserId = _commonService.GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage(MessageConstants.ForbiddenMessage, false);
        }

        var refreshToken = await _repositoryManager.RefreshToken.GetRefreshTokenByIdAsync(refreshTokenId, true);
        if (refreshToken is null)
        {
            return new ResponseMessage(MessageConstants.NotFoundMessage, false);
        }

        refreshToken.IsRevoked = !refreshToken.IsRevoked;
        await _repositoryManager.CommitAsync();

        return new ResponseMessage(MessageConstants.SuccessMessage, true);
    }

    public async Task<ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>> GetAllLoggedInUsers()
    {
        var currentUserId = _commonService.GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>(MessageConstants.ForbiddenMessage, false);
        }

        var refreshTokenCollection = await _repositoryManager.RefreshToken
                .GetAllRefreshTokensAsync();
        if (!refreshTokenCollection.Any())
        {
            return new ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>(MessageConstants.NotFoundMessage, false);
        }         

        var userLoggedInInfoDTOs = refreshTokenCollection.Select(rt => RefreshTokenMapper.RefreshTokenToUserLoggedInInfoDTO(rt));

        return new ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>(MessageConstants.SuccessMessage, true, userLoggedInInfoDTOs);
    }

    public async Task<ResponseMessage<RefreshTokenInfoDTO>> GetRefreshTokenInfoByRefreshTokenId(Guid refreshTokenId)
    {
        var currentUserId = _commonService.GetCurrentUserId();
        var isAdmin = await _repositoryManager.User.IsCurrentUserAdministrator(currentUserId.Value);
        if (!isAdmin)
        {
            return new ResponseMessage<RefreshTokenInfoDTO>(MessageConstants.ForbiddenMessage, false);
        }

        var refreshToken = await _repositoryManager.RefreshToken.GetRefreshTokenByIdAsync(refreshTokenId);
        if (refreshToken is null)
        {
            return new ResponseMessage<RefreshTokenInfoDTO>(MessageConstants.NotFoundMessage, false);
        }

        var refreshTokenInfoDTO = RefreshTokenMapper.RefreshTokenToRefreshTokenInfoDTO(refreshToken);

        return new ResponseMessage<RefreshTokenInfoDTO>(MessageConstants.SuccessMessage, true, refreshTokenInfoDTO);
    }
}
