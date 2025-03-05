using AutoMapper;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IValidator<ReceptionistForCreateDTO> _receptionistForCreateValidator;
    private readonly IValidator<ReceptionistForUpdateDTO> _receptionistForUpdateValidator;

    public ReceptionistService(
            IRepositoryManager repositoryManager, 
            IMapper mapper, 
            IValidator<ReceptionistForCreateDTO> receptionistForCreateValidator, 
            IValidator<ReceptionistForUpdateDTO> receptionistForUpdateValidator)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _receptionistForCreateValidator = receptionistForCreateValidator;
        _receptionistForUpdateValidator = receptionistForUpdateValidator;
    }

    public async Task<ResponseMessage> AddReceptionistAsync(ReceptionistForCreateDTO receptionistForCreateDTO)
    {
        var validationResult = await _receptionistForCreateValidator.ValidateAsync(receptionistForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var receptionist = _mapper.Map<Receptionist>(receptionistForCreateDTO);
        await _repositoryManager.Receptionist.AddReceptionistAsync(receptionist);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeleteReceptionistByIdAsync(Guid receptionistId)
    {
        await _repositoryManager.Receptionist.DeleteReceptionistByIdAsync(receptionistId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<ICollection<ReceptionistTableInfoDTO>>> GetAllReceptionistsAsync()
    {
        var receptionists = await _repositoryManager.Receptionist.GetAllReceptionistsAsync();
        if (receptionists.Count == 0)
        {
            return new ResponseMessage<ICollection<ReceptionistTableInfoDTO>>("No Receptionist's Profiles Found in Database!", 404);
        }

        var receptionistTableInfoDTOs = _mapper.Map<ICollection<ReceptionistTableInfoDTO>>(receptionists);

        return new ResponseMessage<ICollection<ReceptionistTableInfoDTO>>(receptionistTableInfoDTOs);
    }

    public async Task<ResponseMessage<ReceptionistInfoDTO>> GetReceptionistByIdAsync(Guid receptionistId)
    {
        var receptionist = await _repositoryManager.Receptionist.GetReceptionistByIdAsync(receptionistId);
        if (receptionist is null)
        {
            return new ResponseMessage<ReceptionistInfoDTO>("Receptionist's Profile Not Found!", 404);
        }

        var receptionistDTO = _mapper.Map<ReceptionistInfoDTO>(receptionist);

        return new ResponseMessage<ReceptionistInfoDTO>(receptionistDTO);
    }

    public async Task<ResponseMessage> UpdateReceptionistAsync(Guid receptionistId, ReceptionistForUpdateDTO receptionistForUpdateDTO)
    {
        var validationResult = await _receptionistForUpdateValidator.ValidateAsync(receptionistForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var receptionist = await _repositoryManager.Receptionist.GetReceptionistByIdAsync(receptionistId);
        if (receptionist is null)
        {
            return new ResponseMessage("Receptionist's Profile Not Found!", 404);
        }

        receptionist = _mapper.Map(receptionistForUpdateDTO, receptionist);
        await _repositoryManager.Receptionist.UpdateReceptionistAsync(receptionist);

        return new ResponseMessage();
    }
}
