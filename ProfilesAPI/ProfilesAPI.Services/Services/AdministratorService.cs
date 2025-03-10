using AutoMapper;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;
using System.Numerics;

namespace ProfilesAPI.Services.Services;

public class AdministratorService : IAdministratorService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ICommonService _commonService;
    private readonly IBlobStorageService _blobService;
    private readonly IMapper _mapper;
    private readonly IValidator<AdministratorForCreateDTO> _administratorForCreateValidator;
    private readonly IValidator<AdministratorForUpdateDTO> _administratorForUpdateValidator;
    public AdministratorService(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            IValidator<AdministratorForCreateDTO> administratorForCreateValidator,
            IValidator<AdministratorForUpdateDTO> administratorForUpdateValidator,
            ICommonService commonService,
            IBlobStorageService blobService)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _administratorForCreateValidator = administratorForCreateValidator;
        _administratorForUpdateValidator = administratorForUpdateValidator;
        _commonService = commonService;
        _blobService = blobService;
    }

    public async Task<ResponseMessage> AddAdministratorAsync(AdministratorForCreateDTO administratorForCreateDTO, IFormFile file)
    {
        var validationResult = await _administratorForCreateValidator.ValidateAsync(administratorForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        var administrator = _mapper.Map<Administrator>(administratorForCreateDTO);
        using Stream stream = file.OpenReadStream();
        var fileId = await _blobService.UploadAsync(stream, file.ContentType);
        administrator.UserId = currentUserInfo.Id;
        administrator.Photo = fileId;
        await _repositoryManager.Administrator.AddAdministratorAsync(administrator);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeleteAdministratorByIdAsync(Guid administratorId)
    {
        var administrator = await _repositoryManager.Administrator.GetAdministratorByIdAsync(administratorId);
        if (administrator is null)
        {
            return new ResponseMessage("Administrator's Profile Not Found!", 404);
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !administrator.UserId.Equals(currentUserInfo.Id) || !currentUserInfo.Role.Equals(RoleConstants.Administrator)
        {
            return new ResponseMessage("Forbidden Action! You have no rights to manage this Administrator's Profile!", 403);
        }

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

    public async Task<ResponseMessage> UpdateAdministratorAsync(Guid administratorId, AdministratorForUpdateDTO administratorForUpdateDTO, IFormFile? file)
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

        
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !administrator.UserId.Equals(currentUserInfo.Id))
        {
            return new ResponseMessage("Forbidden Action! You have no rights to manage this Administrator's Profile!", 403);
        }

        administrator = _mapper.Map(administratorForUpdateDTO, administrator);
        if (file is not null)
        {
            using Stream stream = file.OpenReadStream();
            await _blobService.DeleteAsync(administrator.Photo);
            var fileId = await _blobService.UploadAsync(stream, file.ContentType);
            administrator.Photo = fileId;
        }

        await _repositoryManager.Administrator.UpdateAdministratorAsync(administrator);

        return new ResponseMessage();
    }
}
