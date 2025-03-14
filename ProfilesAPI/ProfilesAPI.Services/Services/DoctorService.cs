using AutoMapper;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Services;

public class DoctorService : IDoctorService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IBlobStorageService _blobService;
    private readonly ICommonService _commonService;
    private readonly IMapper _mapper;
    private readonly IValidator<DoctorForCreateDTO> _doctorForCreateValidator;
    private readonly IValidator<DoctorForUpdateDTO> _doctorForUpdateValidator;

    public DoctorService(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            IValidator<DoctorForCreateDTO> doctorForCreateValidator,
            IValidator<DoctorForUpdateDTO> doctorForUpdateValidator,
            ICommonService commonService,
            IBlobStorageService blobService)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _doctorForCreateValidator = doctorForCreateValidator;
        _doctorForUpdateValidator = doctorForUpdateValidator;
        _commonService = commonService;
        _blobService = blobService;
    }

    public async Task<ResponseMessage> AddDoctorAsync(DoctorForCreateDTO doctorForCreateDTO)
    {
        var validationResult = await _doctorForCreateValidator.ValidateAsync(doctorForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null)
        {
            return new ResponseMessage("Forbidden Action! You are UnAuthorizaed!", 403);
        }

        var doctor = _mapper.Map<Doctor>(doctorForCreateDTO);
        if(doctorForCreateDTO.Photo is not null)
        {
            using Stream stream = doctorForCreateDTO.Photo.OpenReadStream();
            var blobFileInfo = await _blobService.UploadAsync(stream, doctorForCreateDTO.Photo.ContentType);
            doctor.Photo = blobFileInfo.Uri;
            doctor.PhotoId = blobFileInfo.FileId;
        }

        doctor.UserId = currentUserInfo.Id;
        await _repositoryManager.Doctor.AddDoctorAsync(doctor);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeleteDoctorByIdAsync(Guid doctorId)
    {
        var doctor = await _repositoryManager.Doctor.GetDoctorByIdAsync(doctorId);
        if(doctor is null)
        {
            return new ResponseMessage("Doctor's Profile Not Found!", 404);
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if ((currentUserInfo is null 
            || !doctor.UserId.Equals(currentUserInfo.Id)) 
            && !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action! You have no rights to manage this Doctor's Profile!", 403);
        }

        if(doctor.Photo is not null)
        {
            await _blobService.DeleteAsync(doctor.PhotoId);
        }
        
        await _repositoryManager.Doctor.DeleteDoctorByIdAsync(doctorId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<ICollection<DoctorTableInfoDTO>>> GetAllDoctorsAsync(/*DoctorFilterDTO doctorFilterDTO*/)
    {
        // Pagination
        // Filtration
        // Search

        //if(doctorFilterDTO is not null)
        //{

        //    //var filteredDoctors = await _repositoryManager.Doctor.GetDoctorsByExpression()
        //}
        
        var doctors = await _repositoryManager.Doctor.GetAllDoctorsAsync();
        if (doctors.Count == 0)
        {
            return new ResponseMessage<ICollection<DoctorTableInfoDTO>>("No Doctor's Profiles Found in Database!", 404);
        }

        var doctorTableInfoDTOs = _mapper.Map<ICollection<DoctorTableInfoDTO>>(doctors);

        return new ResponseMessage<ICollection<DoctorTableInfoDTO>>(doctorTableInfoDTOs);
    }
    // no mapping DoctorSpecializations! need to update in DoctorRepository!
    public async Task<ResponseMessage<DoctorInfoDTO>> GetDoctorByIdAsync(Guid doctorId) 
    {
        var doctor = await _repositoryManager.Doctor.GetDoctorByIdAsync(doctorId);
        if (doctor is null)
        {
            return new ResponseMessage<DoctorInfoDTO>("Doctor's Profile Not Found!", 404);
        }

        var doctorInfoDTO = _mapper.Map<DoctorInfoDTO>(doctor);
        
        return new ResponseMessage<DoctorInfoDTO>(doctorInfoDTO);
    }

    public async Task<ResponseMessage> UpdateDoctorAsync(Guid doctorId, DoctorForUpdateDTO doctorForUpdateDTO)
    {
        var validationResult = await _doctorForUpdateValidator.ValidateAsync(doctorForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var doctor = await _repositoryManager.Doctor.GetDoctorByIdAsync(doctorId);
        if (doctor is null)
        {
            return new ResponseMessage("Doctor's Profile Not Found!", 404);
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !doctor.UserId.Equals(currentUserInfo.Id))
        {
            return new ResponseMessage("Forbidden Action! You have no rights to manage this Doctor's Profile!", 403);
        }
        
        doctor = _mapper.Map(doctorForUpdateDTO, doctor);
        if (doctorForUpdateDTO.Photo is not null)
        {
            using Stream stream = doctorForUpdateDTO.Photo.OpenReadStream();
            await _blobService.DeleteAsync(doctor.PhotoId);
            var blobFileInfo = await _blobService.UploadAsync(stream, doctorForUpdateDTO.Photo.ContentType);
            doctor.Photo = blobFileInfo.Uri;
            doctor.PhotoId = blobFileInfo.FileId;
        }

        await _repositoryManager.Doctor.UpdateDoctorAsync(doctor);

        return new ResponseMessage();
    }
}
