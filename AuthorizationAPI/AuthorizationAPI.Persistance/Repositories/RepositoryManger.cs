using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;

namespace AuthorizationAPI.Persistance.Repositories;

public class RepositoryManger : IRepositoryManager
{
    private AuthDBContext _authDBContext;
    private IRoleRepository _roleRepository;
    private IUserRepository _userRepository;
    private IUserStatusRepository _userStatusRepository;
    private IRefreshTokenRepository _refreshTokenRepository;
    public RepositoryManger(AuthDBContext authDBContext)
    {
        _authDBContext = authDBContext;
    }
    public IRoleRepository Role 
    {
        get => _roleRepository ?? new RoleRepository(_authDBContext); 
    }
        
    public IUserStatusRepository UserStatus
    {
        get => _userStatusRepository ?? new UserStatusRepository(_authDBContext);
    }

    public IUserRepository User
    {
        get => _userRepository ?? new UserRepository(_authDBContext);
    }
    public IRefreshTokenRepository RefreshToken
    {
        get => _refreshTokenRepository ?? new RefreshTokenRepository(_authDBContext);
    }

    public async Task Commit()
    {
        await _authDBContext.SaveChangesAsync();
    }
}
