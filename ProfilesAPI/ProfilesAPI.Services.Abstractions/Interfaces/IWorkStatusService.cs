using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Shared.DTOs.WorkStatusDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IWorkStatusService
{
    public Task<ResponseMessage<WorkStatusInfoDTO>> CreateWorkStatusAsync(WorkStatusForCreateDTO workStatusForCreateDTO);
    public Task<ResponseMessage<WorkStatusInfoDTO>> UpdateWorkStatusAsync(Guid workStatusId, WorkStatusForUpdateDTO workStatusForUpdateDTO);
    public Task<ResponseMessage> DeleteWorkStatusByIdAsync(Guid workStatusId);
    public Task<ResponseMessage<WorkStatusInfoDTO>> GetWorkStatusByIdAsync(Guid workStatusId);
    public Task<ResponseMessage<ICollection<WorkStatusTableInfoDTO>>> GetAllWorkStatusesAsync();
}
