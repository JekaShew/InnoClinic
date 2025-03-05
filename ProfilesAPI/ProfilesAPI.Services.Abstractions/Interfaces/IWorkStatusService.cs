using ProfilesAPI.Shared.DTOs.WorkStatusDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IWorkStatusService
{
    public Task AddWorkStatusAsync(WorkStatusForCreateDTO workStatus);
    public Task UpdateWorkStatusAsync(Guid workStatusId, WorkStatusForUpdateDTO updatedWorkStatus);
    public Task DeleteWorkStatusByIdAsync(Guid workStatusId);
    public Task<WorkStatusInfoDTO> GetWorkStatusByIdAsync(Guid workStatusId);
    public Task<ICollection<WorkStatusTableInfoDTO>> GetAllWorkStatusesAsync();
}
