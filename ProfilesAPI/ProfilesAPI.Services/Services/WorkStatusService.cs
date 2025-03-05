using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.WorkStatusDTOs;

namespace ProfilesAPI.Services.Services;

public class WorkStatusService : IWorkStatusService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IValidator<WorkStatusForCreateDTO> _workStatusForCreateValidator;

    public WorkStatusService(
            IRepositoryManager repositoryManager,
            IValidator<WorkStatusForCreateDTO> workStatusForCreateValidator)
    {
        _repositoryManager = repositoryManager;
        _workStatusForCreateValidator = workStatusForCreateValidator;
    }

    public async Task AddWorkStatusAsync(WorkStatusForCreateDTO workStatusForCreateDTO)
    {
        var validationResult = await _workStatusForCreateValidator.ValidateAsync(workStatusForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }


        _repositoryManager.WorkStatus.AddWorkStatusAsync(workStatusForCreateDTO);
    }

    public Task DeleteWorkStatusByIdAsync(Guid workStatusId)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<WorkStatusTableInfoDTO>> GetAllWorkStatusesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<WorkStatusInfoDTO> GetWorkStatusByIdAsync(Guid workStatusId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateWorkStatusAsync(Guid workStatusId, WorkStatusForUpdateDTO updatedWorkStatus)
    {
        throw new NotImplementedException();
    }
}
