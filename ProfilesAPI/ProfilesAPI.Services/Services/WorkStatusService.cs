using AutoMapper;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.WorkStatusDTOs;

namespace ProfilesAPI.Services.Services;

public class WorkStatusService : IWorkStatusService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IValidator<WorkStatusForCreateDTO> _workStatusForCreateValidator;
    private readonly IValidator<WorkStatusForUpdateDTO> _workStatusForUpdateValidator;
    public WorkStatusService(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            IValidator<WorkStatusForCreateDTO> workStatusForCreateValidator,
            IValidator<WorkStatusForUpdateDTO> workStatusForUpdateValidator)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _workStatusForCreateValidator = workStatusForCreateValidator;
        _workStatusForUpdateValidator = workStatusForUpdateValidator;
    }

    public async Task<ResponseMessage> AddWorkStatusAsync(WorkStatusForCreateDTO workStatusForCreateDTO)
    {
        var validationResult = await _workStatusForCreateValidator.ValidateAsync(workStatusForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var workStatus = _mapper.Map<WorkStatus>(workStatusForCreateDTO);
        await _repositoryManager.WorkStatus.AddWorkStatusAsync(workStatus);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeleteWorkStatusByIdAsync(Guid workStatusId)
    {
        await _repositoryManager.WorkStatus.DeleteWorkStatusByIdAsync(workStatusId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<ICollection<WorkStatusTableInfoDTO>>> GetAllWorkStatusesAsync()
    {
        var workStatuses = await _repositoryManager.WorkStatus.GetAllWorkStatusesAsync();
        if(workStatuses.Count == 0)
        {
            return new ResponseMessage<ICollection<WorkStatusTableInfoDTO>>("No Work Statuses Found in Database!", 404);
        }

        var workStatusTableInfoDTOs = _mapper.Map<ICollection<WorkStatusTableInfoDTO>>(workStatuses);

        return new ResponseMessage<ICollection<WorkStatusTableInfoDTO>>(workStatusTableInfoDTOs);
    }

    public async Task<ResponseMessage<WorkStatusInfoDTO>> GetWorkStatusByIdAsync(Guid workStatusId)
    {
        var workStatus = await _repositoryManager.WorkStatus.GetWorkStatusByIdAsync(workStatusId);
        if(workStatus is null)
        {
            return new ResponseMessage<WorkStatusInfoDTO>("Work Status Not Found!", 404);
        }

        var workStatusInfoDTO = _mapper.Map<WorkStatusInfoDTO>(workStatus);

        return new ResponseMessage<WorkStatusInfoDTO>(workStatusInfoDTO);
    }

    public async Task<ResponseMessage> UpdateWorkStatusAsync(Guid workStatusId, WorkStatusForUpdateDTO workStatusForUpdateDTO)
    {
        var validationResult = await _workStatusForUpdateValidator.ValidateAsync(workStatusForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var workStatus = await _repositoryManager.WorkStatus.GetWorkStatusByIdAsync(workStatusId);
        if(workStatus is null)
        {
            return new ResponseMessage("Work Status Not Found!", 404);
        }

        workStatus = _mapper.Map(workStatusForUpdateDTO, workStatus);
        await _repositoryManager.WorkStatus.UpdateWorkStatusAsync(workStatusId, workStatus);

        return new ResponseMessage();
    }
}
