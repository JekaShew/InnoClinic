using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Persistance.Data;

namespace OfficesAPI.Persistance.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private IOfficesContext _officesContext;
    private IOfficeRepository _officeRepository;
    private IPhotoRepository _photoRepository;
    public RepositoryManager(IOfficesContext officeContext)
    {
        _officesContext = officeContext;
    }
    public IOfficeRepository Office
    {
        get => _officeRepository ?? new OfficeRepository(_officesContext);
    }

    public IPhotoRepository Photo
    {
        get => _photoRepository ?? new PhotoRepository(_officesContext);
    }

    public async Task TransactionExecution()
    {
        await _officesContext.TransactionExecution();
    }

    public async Task SingleExecution()
    {
        await _officesContext.SingleExecution();
    }

    public void Dispose()
    {
        _officesContext.Dispose();
    }
}
