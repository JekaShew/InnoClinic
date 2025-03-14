using AutoMapper;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Services;

public class PatientService : IPatientService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IBlobStorageService _blobService;
    private readonly ICommonService _commonService;
    private readonly IMapper _mapper;
    private readonly IValidator<PatientForCreateDTO> _patientForCreateValidator;
    private readonly IValidator<PatientForUpdateDTO> _patientForUpdateValidator;

    public PatientService(
            IRepositoryManager repositoryManager,
            IBlobStorageService blobService,
            ICommonService commonService,
            IMapper mapper,
            IValidator<PatientForCreateDTO> patientForCreateValidator,
            IValidator<PatientForUpdateDTO> patientForUpdateValidator)
    {
        _repositoryManager = repositoryManager;
        _commonService = commonService;
        _blobService = blobService;
        _mapper = mapper;
        _patientForCreateValidator = patientForCreateValidator;
        _patientForUpdateValidator = patientForUpdateValidator;
        
    }

    public async Task<ResponseMessage> AddPatientAsync(PatientForCreateDTO patientForCreateDTO)
    {
        var validationResult = await _patientForCreateValidator.ValidateAsync(patientForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if(currentUserInfo is null)
        {
            return new ResponseMessage("Forbidden Action! You are UnAuthorizaed!", 403);
        }

        var patient = _mapper.Map<Patient>(patientForCreateDTO);
        if (patientForCreateDTO.Photo is not null)
        {
            using Stream stream = patientForCreateDTO.Photo.OpenReadStream();
            var blobFileInfo = await _blobService.UploadAsync(stream, patientForCreateDTO.Photo.ContentType);
            patient.Photo = blobFileInfo.Uri;
            patient.PhotoId = blobFileInfo.FileId;
        }
       
        patient.UserId = currentUserInfo.Id;
        await _repositoryManager.Patient.AddPatientAsync(patient);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeletePatientByIdAsync(Guid patientId)
    {
        var patient = await _repositoryManager.Patient.GetPatientByIdAsync(patientId);
        if(patient is null)
        {
            return new ResponseMessage("Patient's Profile Not Found!", 404);
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if ((currentUserInfo is null
            || !patient.UserId.Equals(currentUserInfo.Id))
            && !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action! You have no rights to manage this Patient's Profile!", 403);
        }

        if(patient.Photo is not null)
        {
            await _blobService.DeleteAsync(patient.PhotoId);
        }
        
        await _repositoryManager.Patient.DeletePatientByIdAsync(patientId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<ICollection<PatientTableInfoDTO>>> GetAllPatientsAsync()
    {
        // Pagination
        // Filtration
        // Search

        var patients = await _repositoryManager.Patient.GetAllPatientsAsync();
        if (patients.Count == 0)
        {
            return new ResponseMessage<ICollection<PatientTableInfoDTO>>("No Patient's Profiles Found in Database!", 404);
        }

        var patientsTableInfoDTOs = _mapper.Map<ICollection<PatientTableInfoDTO>>(patients);

        return new ResponseMessage<ICollection<PatientTableInfoDTO>>(patientsTableInfoDTOs);
    }

    public async Task<ResponseMessage<PatientInfoDTO>> GetPatientByIdAsync(Guid patientId)
    {
        var patient = await _repositoryManager.Patient.GetPatientByIdAsync(patientId);
        if (patient is null)
        {
            return new ResponseMessage<PatientInfoDTO>("Patient's Profile Not Found!", 404);
        }

        var patientInfoDTO = _mapper.Map<PatientInfoDTO>(patient);
      
        return new ResponseMessage<PatientInfoDTO>(patientInfoDTO);
    }

    public async Task<ResponseMessage> UpdatePatientAsync(Guid patientId, PatientForUpdateDTO patientForUpdateDTO)
    {
        var validationResult = await _patientForUpdateValidator.ValidateAsync(patientForUpdateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var patient = await _repositoryManager.Patient.GetPatientByIdAsync(patientId);
        if (patient is null)
        {
            return new ResponseMessage("Patient's Profile Not Found!", 404);
        }

        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !patient.UserId.Equals(currentUserInfo.Id))
        {
            return new ResponseMessage("Forbidden Action! You have no rights to manage this Patient's Profile!", 403);
        }

        patient = _mapper.Map(patientForUpdateDTO, patient);
        if(patientForUpdateDTO.Photo is not null)
        {
            using Stream stream = patientForUpdateDTO.Photo.OpenReadStream();
            await _blobService.DeleteAsync(patient.PhotoId);
            var blobFileInfo = await _blobService.UploadAsync(stream, patientForUpdateDTO.Photo.ContentType);
            patient.Photo = blobFileInfo.Uri;
            patient.PhotoId = blobFileInfo.FileId;
        }
        
        await _repositoryManager.Patient.UpdatePatientAsync(patient);

        return new ResponseMessage();
    }
}
