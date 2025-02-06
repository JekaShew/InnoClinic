namespace AuthorizationAPI.Domain.IRepositories;

public interface IRepositoryManager
{
    IRoleRepository Role { get; }
    IUserStatusRepository UserStatus { get; }
    IUserRepository User { get; }
    IRefreshTokenRepository RefreshToken { get; }
    Task SaveChangesAsync();
}
