using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthorizationAPI.Persistance.Repositories;

public class EFCoreRepositoryManger : IRepositoryManager
{
    private AuthDBContext _authDBContext;
    private IDbContextTransaction _transaction;
    private IRoleRepository _roleRepository;
    private IUserRepository _userRepository;
    private IUserStatusRepository _userStatusRepository;
    private IRefreshTokenRepository _refreshTokenRepository;
    public EFCoreRepositoryManger(AuthDBContext authDBContext)
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

    public async Task BeginAsync()
    {
        _transaction = _transaction ?? await _authDBContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _authDBContext.SaveChangesAsync();
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackAsync();
            throw new Exception();
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        } 
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
