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
        get
        {
            if (_roleRepository is null)
                _roleRepository = new RoleRepository(_authDBContext);

            return _roleRepository;
        }
    }

    public IUserStatusRepository UserStatus
    {
        get
        {
            if (_userStatusRepository is null)
                _userStatusRepository = new UserStatusRepository(_authDBContext);

            return _userStatusRepository;
        }
    }

    public IUserRepository User
    {
        get
        {
            if (_userRepository is null)
                _userRepository = new UserRepository(_authDBContext);

            return _userRepository;
        }
    }
    public IRefreshTokenRepository RefreshToken
    {
        get
        {
            if (_refreshTokenRepository is null)
                _refreshTokenRepository = new RefreshTokenRepository(_authDBContext);

            return _refreshTokenRepository;
        }
    }

    public async Task SaveChangesAsync()
    {
        await _authDBContext.SaveChangesAsync();
    }
}
