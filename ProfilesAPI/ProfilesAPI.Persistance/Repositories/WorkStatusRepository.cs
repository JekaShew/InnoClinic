using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class WorkStatusRepository : IWorkStatusRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public WorkStatusRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task AddWorkStatusAsync(WorkStatus workStatus)
    {
        await _profilesDBContext.Connection.InsertAsync<WorkStatus>(workStatus);
    }

    public async Task DeleteWorkStatusByIdAsync(Guid workStatusId)
    {
        await _profilesDBContext.Connection.DeleteAsync<WorkStatus>(new WorkStatus { Id = workStatusId });
    }

    public async Task<ICollection<WorkStatus>> GetAllWorkStatusesAsync()
    {
        var workStatuses = await _profilesDBContext.Connection.GetAllAsync<WorkStatus>();

        return workStatuses.ToList();
    }

    public async Task<WorkStatus> GetWorkStatusByIdAsync(Guid workStatusId)
    {
        var workStatus =  await _profilesDBContext.Connection.GetAsync<WorkStatus>(workStatusId);
        
        return workStatus;
    }

    public async Task UpdateWorkStatusAsync(WorkStatus updatedWorkStatus)
    {
        await _profilesDBContext.Connection.UpdateAsync<WorkStatus>(updatedWorkStatus);
    }
}
