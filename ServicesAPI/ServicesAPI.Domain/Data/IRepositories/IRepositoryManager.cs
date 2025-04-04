using ServicesAPI.Domain.Data.Models;

namespace ServicesAPI.Domain.Data.IRepositories;

public interface IRepositoryManager
{
    ISpecializationRepository Specialization { get; }
    IServiceReposiotry Service { get; }
    IServiceCategoryRepository ServiceCategory{ get; }
    IServiceCategorySpecializationRepository ServiceCategorySpecialization{ get; }
    abstract Task CommitAsync();
    abstract Task RollbackAsync();
    abstract Task BeginAsync();
}
