using Microsoft.EntityFrameworkCore.Storage;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Persistance.Data;

namespace ServicesAPI.Persistance.Repositories;

public class EFCoreRepositoryManager : IRepositoryManager
{
    private ServicesDBContext _servicesDBContext;
    private IDbContextTransaction _transaction;
    private IServiceReposiotry _serviceRepository;
    private ISpecializationRepository _specializationRepository;
    private IServiceCategoryRepository _serviceCategory;
    private IServiceCategorySpecializationRepository _serviceCategorySpecialization;

    public EFCoreRepositoryManager(ServicesDBContext servicesDBContext)
    {
        _servicesDBContext = servicesDBContext;
    }

    public ISpecializationRepository Specialization
    {
        get => _specializationRepository ?? new SpecializationRepository(_servicesDBContext);
    }

    public IServiceReposiotry Service
    {
        get => _serviceRepository ?? new ServiceRepository(_servicesDBContext);
    }

    public IServiceCategoryRepository ServiceCategory
    {
        get => _serviceCategory ?? new ServiceCategoryRepository(_servicesDBContext);
    }

    public IServiceCategorySpecializationRepository ServiceCategorySpecialization
    {
        get => _serviceCategorySpecialization ?? new ServiceCategorySpecializationRepository(_servicesDBContext);
    }

    public async Task BeginAsync()
    {
        _transaction = _transaction ?? await _servicesDBContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _servicesDBContext.SaveChangesAsync();
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            await RollbackAsync();
            throw new Exception(ex.Message, ex.InnerException);
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
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
