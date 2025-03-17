using AutoMapper;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
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
    private readonly ICommonService _commonService;
    private readonly IBlobStorageService _blobService;
    private readonly IMapper _mapper;
    private readonly IValidator<ReceptionistForCreateDTO> _receptionistForCreateValidator;
    private readonly IValidator<ReceptionistForUpdateDTO> _receptionistForUpdateValidator;

    public ReceptionistService(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            IValidator<ReceptionistForCreateDTO> receptionistForCreateValidator,
            IValidator<ReceptionistForUpdateDTO> receptionistForUpdateValidator,
            ICommonService commonService,
            IBlobStorageService blobService)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _receptionistForCreateValidator = receptionistForCreateValidator;
        _receptionistForUpdateValidator = receptionistForUpdateValidator;
        _commonService = commonService;
        _blobService = blobService;
    }

    public async Task<ResponseMessage> AddReceptionistAsync(ReceptionistForCreateDTO receptionistForCreateDTO)
    {
        var validationResult = await _receptionistForCreateValidator.ValidateAsync(receptionistForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null)
        {
            return new ResponseMessage("Forbidden Action! You are UnAuthorizaed!", 403);
        }

        var receptionist = _mapper.Map<Receptionist>(receptionistForCreateDTO);
        if (receptionistForCreateDTO.Photo is not null)
        {
            using Stream stream = receptionistForCreateDTO.Photo.OpenReadStream();
            var blobFileInfo = await _blobService.UploadAsync(stream, receptionistForCreateDTO.Photo.ContentType);
            receptionist.Photo = blobFileInfo.Uri;
            receptionist.PhotoId = blobFileInfo.FileId;
        }
       
        receptionist.UserId = currentUserInfo.Id;
        await _repositoryManager.Receptionist.AddReceptionistAsync(receptionist);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeleteReceptionistByIdAsync(Guid receptionistId)
    {
        var receptionist = await _repositoryManager.Receptionist.GetReceptionistByIdAsync(receptionistId);
        if (receptionist is null)
        {
            return new ResponseMessage("Receptionist's Profile Not Found!", 404);
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if ((currentUserInfo is null
            || !receptionist.UserId.Equals(currentUserInfo.Id))
            && !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action! You have no rights to manage this Receptionist's Profile!", 403);
        }

        if (receptionist.Photo is not null)
        {
            await _blobService.DeleteAsync(receptionist.PhotoId);
        }
        
        await _repositoryManager.Receptionist.DeleteReceptionistByIdAsync(receptionistId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<ICollection<ReceptionistTableInfoDTO>>> GetAllReceptionistsAsync()
    {
        // Pagination
        // Filtration
        // Search

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

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !receptionist.UserId.Equals(currentUserInfo.Id))
        {
            return new ResponseMessage("Forbidden Action! You have no rights to manage this Administrator's Profile!", 403);
        }

        receptionist = _mapper.Map(receptionistForUpdateDTO, receptionist);
        if (receptionistForUpdateDTO.Photo is not null)
        {
            using Stream stream = receptionistForUpdateDTO.Photo.OpenReadStream();
            await _blobService.DeleteAsync(receptionist.PhotoId);
            var blobFileInfo = await _blobService.UploadAsync(stream, receptionistForUpdateDTO.Photo.ContentType);
            receptionist.Photo = blobFileInfo.Uri;
            receptionist.PhotoId = blobFileInfo.FileId;
        }

        await _repositoryManager.Receptionist.UpdateReceptionistAsync(receptionistId, receptionist);

        return new ResponseMessage();
    }
}
