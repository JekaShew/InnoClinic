using ProfilesAPI.Domain.Data.Models;

namespace ProfilesAPI.Domain.IRepositories;

public interface IWorkStatusRepository
{
    public Task AddWorkStatusAsync(WorkStatus workStatus);
    public Task UpdateWorkStatusAsync(WorkStatus updatedWorkStatus);
    public Task DeleteWorkStatusByIdAsync(Guid workStatusId);
    public Task<WorkStatus> GetWorkStatusByIdAsync(Guid workStatusId);
    public Task<ICollection<WorkStatus>> GetAllWorkStatusesAsync();
}
