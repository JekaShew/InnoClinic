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
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Services;
// think about specializations!!!!!
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

    public async Task<ResponseMessage> AddDoctorAsync(DoctorForCreateDTO doctorForCreateDTO, IFormFile file)
    {
        var validationResult = await _doctorForCreateValidator.ValidateAsync(doctorForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        var doctor = _mapper.Map<Doctor>(doctorForCreateDTO);
        using Stream stream = file.OpenReadStream();
        var fileId = await _blobService.UploadAsync(stream, file.ContentType);
        doctor.UserId = currentUserInfo.Id;
        doctor.Photo = fileId;

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
        if (currentUserInfo is null || !doctor.UserId.Equals(currentUserInfo.Id) || !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action! You have no rights to manage this Doctor's Profile!", 403);
        }

        await _repositoryManager.Doctor.DeleteDoctorByIdAsync(doctorId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<ICollection<DoctorTableInfoDTO>>> GetAllDoctorsAsync(/*DoctorFilterDTO doctorFilterDTO*/)
    {
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

    public async Task<ResponseMessage> UpdateDoctorAsync(Guid doctorId, DoctorForUpdateDTO doctorForUpdateDTO, IFormFile? file)
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
        if (file is not null)
        {
            using Stream stream = file.OpenReadStream();
            await _blobService.DeleteAsync(doctor.Photo);
            var fileId = await _blobService.UploadAsync(stream, file.ContentType);
            doctor.Photo = fileId;
        }

        await _repositoryManager.Doctor.UpdateDoctorAsync(doctor);

        return new ResponseMessage();
    }
}
