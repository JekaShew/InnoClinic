using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly IValidator<PatientForCreateDTO> _patientForCreateValidator;
    private readonly IValidator<PatientForUpdateDTO> _patientForUpdateValidator;

    public PatientService(
            IRepositoryManager repositoryManager,
            IMapper mapper, 
            IValidator<PatientForCreateDTO> patientForCreateValidator, 
            IValidator<PatientForUpdateDTO> patientForUpdateValidator)
    {
        _repositoryManager = repositoryManager;
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

        var patient = _mapper.Map<Patient>(patientForCreateDTO);
        await _repositoryManager.Patient.AddPatientAsync(patient);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeletePatientByIdAsync(Guid patientId)
    {
        await _repositoryManager.Patient.DeletePatientByIdAsync(patientId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<ICollection<PatientTableInfoDTO>>> GetAllPatientsAsync()
    {
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

        patient = _mapper.Map(patientForUpdateDTO, patient);
        await _repositoryManager.Patient.UpdatePatientAsync(patient);

        return new ResponseMessage();
    }
}
