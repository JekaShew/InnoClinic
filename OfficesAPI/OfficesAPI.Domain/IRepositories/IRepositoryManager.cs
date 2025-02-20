namespace OfficesAPI.Domain.IRepositories;

public interface IRepositoryManager : IDisposable
{
    IOfficeRepository Office { get; }
    IPhotoRepository Photo { get; }
    abstract Task SingleExecution();
    abstract Task TransactionExecution();
}
