using ProfilesAPI.Domain.Data.Models;

namespace ProfilesAPI.Domain.IRepositories;

public interface IWorkStatusRepository
{
    public Task CreateAsync(WorkStatus workStatus);
    public Task UpdateAsync(Guid workStatusId, WorkStatus updatedWorkStatus);
    public Task DeleteByIdAsync(Guid workStatusId);
    public Task<WorkStatus> GetByIdAsync(Guid workStatusId);
    public Task<ICollection<WorkStatus>> GetAllAsync();
}
