using AuthorizationAPI.Domain.IRepositories;

namespace AuthorizationAPI.Services.Extensions;

public class BackgroundTasks
{
    private readonly IRepositoryManager _repositoryManager;
    public BackgroundTasks(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task CleanExpiredRefreshTokensAsync()
    {
        var expiredRefreshTokens = await _repositoryManager.RefreshToken.GetAllExpiredRefreshTokensAsync();
        foreach (var refreshToken in expiredRefreshTokens)
        {
            _repositoryManager.RefreshToken.DeleteRefreshToken(refreshToken);
        }

        await _repositoryManager.CommitAsync();
    }
}