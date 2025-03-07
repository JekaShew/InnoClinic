using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
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
        var refreshToken = await _repositoryManager.RefreshToken.GetRefreshTokenByIdAsync(refreshTokenId);
        if (refreshToken is null)
        {
            return new ResponseMessage("Refresh Token not Found!", 404);
        }

        _repositoryManager.RefreshToken.DeleteRefreshToken(refreshToken);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> RevokeRefreshTokenByRefreshTokenId(Guid refreshTokenId)
    {
        var refreshToken = await _repositoryManager.RefreshToken.GetRefreshTokenByIdAsync(refreshTokenId);
        if (refreshToken is null)
        {
            return new ResponseMessage("Refresh Token not Found!", 404);
        }

        refreshToken.IsRevoked = !refreshToken.IsRevoked;
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>> GetAllLoggedInUsers()
    {
        var refreshTokenCollection = await _repositoryManager.RefreshToken
                .GetAllRefreshTokensAsync();
        if (!refreshTokenCollection.Any())
        {
            return new ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>("No Logged in Users Found!", 404);
        }         

        var userLoggedInInfoDTOs = refreshTokenCollection.Select(rt => RefreshTokenMapper.RefreshTokenToUserLoggedInInfoDTO(rt));

        return new ResponseMessage<IEnumerable<UserLoggedInInfoDTO>>(userLoggedInInfoDTOs);
    }

    public async Task<ResponseMessage<RefreshTokenInfoDTO>> GetRefreshTokenInfoByRefreshTokenId(Guid refreshTokenId)
    {
        var refreshToken = await _repositoryManager.RefreshToken.GetRefreshTokenByIdAsync(refreshTokenId);
        if (refreshToken is null)
        {
            return new ResponseMessage<RefreshTokenInfoDTO>("Refresh Token not Found!", 404);
        }

        var refreshTokenInfoDTO = RefreshTokenMapper.RefreshTokenToRefreshTokenInfoDTO(refreshToken);

        return new ResponseMessage<RefreshTokenInfoDTO>(refreshTokenInfoDTO);
    }
}
