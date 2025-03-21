using AutoMapper;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Services.Validators.DoctorValidators;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Services;

public class AdministratorService : IAdministratorService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ICommonService _commonService;
    private readonly IBlobStorageService _blobService;
    private readonly IMapper _mapper;
    private readonly IValidator<AdministratorForCreateDTO> _administratorForCreateValidator;
    private readonly IValidator<AdministratorForUpdateDTO> _administratorForUpdateValidator;
    private readonly IValidator<AdministratorParameters> _administratorParametersValidator;

    public AdministratorService(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            ICommonService commonService,
            IBlobStorageService blobService,
            IValidator<AdministratorForCreateDTO> administratorForCreateValidator,
            IValidator<AdministratorForUpdateDTO> administratorForUpdateValidator,
            IValidator<AdministratorParameters> administratorParametersValidator)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _commonService = commonService;
        _blobService = blobService;
        _administratorForCreateValidator = administratorForCreateValidator;
        _administratorForUpdateValidator = administratorForUpdateValidator;
        _administratorParametersValidator = administratorParametersValidator;
    }

    public async Task<ResponseMessage<Guid>> CreateAdministratorAsync(AdministratorForCreateDTO administratorForCreateDTO)
    {
        var validationResult = await _administratorForCreateValidator.ValidateAsync(administratorForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null)
        {
            return new ResponseMessage<Guid>("Forbidden Action! You are UnAuthorizaed!", 403);
        }

        var isProfileExists = await _repositoryManager.Administrator.IsProfileExists(currentUserInfo.Id);
        if (isProfileExists)
        {
            return new ResponseMessage<Guid>("Error! This profile already exists!", 400);
        }

        var administrator = _mapper.Map<Administrator>(administratorForCreateDTO);
        if (administratorForCreateDTO.Photo is not null)
        {
            using Stream stream = administratorForCreateDTO.Photo.OpenReadStream();
            var blobFileInfo = await _blobService.UploadAsync(stream, administratorForCreateDTO.Photo.ContentType);
            administrator.Photo = blobFileInfo.Uri;
            administrator.PhotoId = blobFileInfo.FileId;
        }
        
        administrator.UserId = currentUserInfo.Id;
        var administratorId = await _repositoryManager.Administrator.CreateAsync(administrator);

        return new ResponseMessage<Guid>(administratorId);
    }

    public async Task<ResponseMessage> DeleteAdministratorByIdAsync(Guid administratorId)
    {
        var administrator = await _repositoryManager.Administrator.GetByIdAsync(administratorId);
        if (administrator is null)
        {
            return new ResponseMessage("Administrator's Profile Not Found!", 404);
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();

        if (currentUserInfo is null || (!administrator.UserId.Equals(currentUserInfo.Id) && 
                                        !currentUserInfo.Role.Equals(RoleConstants.Administrator)))
        {
            return new ResponseMessage("Forbidden Action! You have no rights to manage this Administrator's Profile!", 403);
        }
        
        if (administrator.Photo is not null)
        {
            await _blobService.DeleteAsync(administrator.PhotoId);
        }

        await _repositoryManager.Administrator.DeleteByIdAsync(administratorId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<AdministratorInfoDTO>> GetAdministratorByIdAsync(Guid administratorId)
    {
        var administrator = await _repositoryManager.Administrator.GetByIdAsync(administratorId);
        if (administrator is null)
        {
            return new ResponseMessage<AdministratorInfoDTO>("Administrator's Profile Not Found!", 404);
        }

        var administratorInfoDTO = _mapper.Map<AdministratorInfoDTO>(administrator);

        return new ResponseMessage<AdministratorInfoDTO>(administratorInfoDTO);
    }

    public async Task<ResponseMessage<ICollection<AdministratorTableInfoDTO>>> GetAllAdministratorsAsync(AdministratorParameters? administratorParameters)
    {
        // Pagination
        // Filtration
        // Search
        if (administratorParameters is not null)
        {
            var validationResult = await _administratorParametersValidator.ValidateAsync(administratorParameters);
            if (!validationResult.IsValid)
            {
                throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
            }
        }

        var administrators = await _repositoryManager.Administrator.GetAllAsync(administratorParameters);
        if (administrators.Count == 0)
        {
            return new ResponseMessage<ICollection<AdministratorTableInfoDTO>>("No Administrator's Profiles Found in Database!", 404);
        }

        var administratorTableInfoDTOs = _mapper.Map<ICollection<AdministratorTableInfoDTO>>(administrators);

        return new ResponseMessage<ICollection<AdministratorTableInfoDTO>>(administratorTableInfoDTOs);
    }

    public async Task<ResponseMessage<AdministratorInfoDTO>> UpdateAdministratorAsync(Guid administratorId, AdministratorForUpdateDTO administratorForUpdateDTO)
    {
        var validationResult = await _administratorForUpdateValidator.ValidateAsync(administratorForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var administrator = await _repositoryManager.Administrator.GetByIdAsync(administratorId);
        if (administrator is null)
        {
            return new ResponseMessage<AdministratorInfoDTO>("Administrator's Profile Not Found!", 404);
        }
        
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        var currentUserInfoCheck =
            currentUserInfo is null ? false :
            administrator.UserId.Equals(currentUserInfo.Id) ? true : false;
        if (currentUserInfo is null || !currentUserInfo.Id.Equals(administrator.UserId))
        {
            return new ResponseMessage<AdministratorInfoDTO>("Forbidden Action! You have no rights to manage this Administrator's Profile!", 403);
        }

        administrator = _mapper.Map(administratorForUpdateDTO, administrator);
        if (administratorForUpdateDTO.Photo is not null)
        {
            using Stream stream = administratorForUpdateDTO.Photo.OpenReadStream();
            await _blobService.DeleteAsync(administrator.PhotoId);
            var blobFileInfo = await _blobService.UploadAsync(stream, administratorForUpdateDTO.Photo.ContentType);
            administrator.Photo = blobFileInfo.Uri;
            administrator.PhotoId = blobFileInfo.FileId;
        }
        else
        {
            await _blobService.DeleteAsync(administrator.PhotoId);
            administrator.Photo = null;
            administrator.PhotoId = Guid.Empty;
        }

        await _repositoryManager.Administrator.UpdateAsync(administratorId, administrator);
        var administratorInfoDTO = _mapper.Map<AdministratorInfoDTO>(administrator);
        return new ResponseMessage<AdministratorInfoDTO>(administratorInfoDTO);
    }
}
