using AutoMapper;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Services;

public class AdministratorService : IAdministratorService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IValidator<AdministratorForCreateDTO> _administratorForCreateValidator;
    private readonly IValidator<AdministratorForUpdateDTO> _administratorForUpdateValidator;
    public AdministratorService(
            IRepositoryManager repositoryManager, 
            IMapper mapper, 
            IValidator<AdministratorForCreateDTO> administratorForCreateValidator, 
            IValidator<AdministratorForUpdateDTO> administratorForUpdateValidator)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _administratorForCreateValidator = administratorForCreateValidator;
        _administratorForUpdateValidator = administratorForUpdateValidator;
    }

    public async Task<ResponseMessage> AddAdministratorAsync(AdministratorForCreateDTO administratorForCreateDTO)
    {
        var validationResult = await _administratorForCreateValidator.ValidateAsync(administratorForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var administrator = _mapper.Map<Administrator>(administratorForCreateDTO);
        await _repositoryManager.Administrator.AddAdministratorAsync(administrator);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeleteAdministratorByIdAsync(Guid administratorId)
    {
        await _repositoryManager.Administrator.DeleteAdministratorByIdAsync(administratorId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<AdministratorInfoDTO>> GetAdministratorByIdAsync(Guid administratorId)
    {
        var administrator = await _repositoryManager.Administrator.GetAdministratorByIdAsync(administratorId);
        if (administrator is null)
        {
            return new ResponseMessage<AdministratorInfoDTO>("Administrator's Profile Not Found!", 404);
        }

        var administratorInfoDTO = _mapper.Map<AdministratorInfoDTO>(administrator);

        return new ResponseMessage<AdministratorInfoDTO>(administratorInfoDTO);
    }

    public async Task<ResponseMessage<ICollection<AdministratorTableInfoDTO>>> GetAllAdministratorsAsync()
    {
        var administrators = await _repositoryManager.Administrator.GetAllAdministratorsAsync();
        if (administrators.Count == 0)
        {
            return new ResponseMessage<ICollection<AdministratorTableInfoDTO>>("No Administrator's Profiles Found in Database!", 404);
        }

        var administratorTableInfoDTOs = _mapper.Map<ICollection<AdministratorTableInfoDTO>>(administrators);

        return new ResponseMessage<ICollection<AdministratorTableInfoDTO>>(administratorTableInfoDTOs);
    }

    public async Task<ResponseMessage> UpdateAdministratorAsync(Guid administratorId, AdministratorForUpdateDTO administratorForUpdateDTO)
    {
        var validationResult = await _administratorForUpdateValidator.ValidateAsync(administratorForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var administrator = await _repositoryManager.Administrator.GetAdministratorByIdAsync(administratorId);
        if (administrator is null)
        {
            return new ResponseMessage("Administrator's Profile Not Found!", 404);
        }

        administrator = _mapper.Map(administratorForUpdateDTO, administrator);
        await _repositoryManager.Administrator.UpdateAdministratorAsync(administrator);

        return new ResponseMessage();
    }
}
