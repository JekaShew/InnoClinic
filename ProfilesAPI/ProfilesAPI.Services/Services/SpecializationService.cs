using AutoMapper;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Services.Services;

public class SpecializationService : ISpecializationService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IValidator<SpecializationForCreateDTO> _specializationForCreateValidator;
    private readonly IValidator<SpecializationForUpdateDTO> _specializationForUpdateValidator;

    public SpecializationService(
            IRepositoryManager repositoryManager, 
            IMapper mapper,
            IValidator<SpecializationForCreateDTO> specializationForCreateValidator, 
            IValidator<SpecializationForUpdateDTO> specializationForUpdateValidator)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _specializationForCreateValidator = specializationForCreateValidator;
        _specializationForUpdateValidator = specializationForUpdateValidator;
    }

    public async Task<ResponseMessage> AddSpecializationAsync(SpecializationForCreateDTO specializationForCreateDTO)
    {
        var validationResult = await _specializationForCreateValidator.ValidateAsync(specializationForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var specialization = _mapper.Map<Specialization>(specializationForCreateDTO);
        await _repositoryManager.Specialization.AddSpecializationAsync(specialization);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeleteSpecializationByIdAsync(Guid specializationId)
    {
        await _repositoryManager.Specialization.DeleteSpecializationByIdAsync(specializationId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<ICollection<SpecializationTableInfoDTO>>> GetAllSpecializationsAsync()
    {
        var specializations = await _repositoryManager.Specialization.GetAllSpecializationsAsync();
        if (specializations.Count == 0)
        {
            return new ResponseMessage<ICollection<SpecializationTableInfoDTO>>("No Specializations Found in Database!", 404);
        }

        var specializationTableInfoDTOs = _mapper.Map<ICollection<SpecializationTableInfoDTO>>(specializations);

        return new ResponseMessage<ICollection<SpecializationTableInfoDTO>>(specializationTableInfoDTOs);
    }

    public async Task<ResponseMessage<SpecializationInfoDTO>> GetSpecializationByIdAsync(Guid specializationId)
    {
        var specialization = await _repositoryManager.Specialization.GetSpecializationByIdAsync(specializationId);
        if (specialization is null)
        {
            return new ResponseMessage<SpecializationInfoDTO>("Specialization Not Found!", 404);
        }

        var specializationInfoDTO = _mapper.Map<SpecializationInfoDTO>(specialization);

        return new ResponseMessage<SpecializationInfoDTO>(specializationInfoDTO);
    }

    public async Task<ResponseMessage> UpdateSpecializationAsync(Guid specializationId, SpecializationForUpdateDTO specializationForUpdateDTO)
    {
        var validationResult = await _specializationForUpdateValidator.ValidateAsync(specializationForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var specialization = await _repositoryManager.Specialization.GetSpecializationByIdAsync(specializationId);
        if (specialization is null)
        {
            return new ResponseMessage("Specialization Not Found!", 404);
        }

        specialization = _mapper.Map(specializationForUpdateDTO, specialization);
        await _repositoryManager.Specialization.UpdateSpecializationAsync(specialization);

        return new ResponseMessage();
    }
}
