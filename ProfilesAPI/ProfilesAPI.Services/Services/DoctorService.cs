using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly IValidator<DoctorForCreateDTO> _doctorForCreateValidator;
    private readonly IValidator<DoctorForUpdateDTO> _doctorForUpdateValidator;

    public DoctorService(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            IValidator<DoctorForCreateDTO> doctorForCreateValidator, 
            IValidator<DoctorForUpdateDTO> doctorForUpdateValidator)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _doctorForCreateValidator = doctorForCreateValidator;
        _doctorForUpdateValidator = doctorForUpdateValidator;
    }

    public async Task<ResponseMessage> AddDoctorAsync(DoctorForCreateDTO doctorForCreateDTO)
    {
        var validationResult = await _doctorForCreateValidator.ValidateAsync(doctorForCreateDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var doctor = _mapper.Map<Doctor>(doctorForCreateDTO);
        await _repositoryManager.Doctor.AddDoctorAsync(doctor);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage> DeleteDoctorByIdAsync(Guid doctorId)
    {
        await _repositoryManager.Doctor.DeleteDoctorByIdAsync(doctorId);

        return new ResponseMessage();
    }

    public async Task<ResponseMessage<ICollection<DoctorTableInfoDTO>>> GetAllDoctorsAsync()
    {
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

        doctor = _mapper.Map(doctorForUpdateDTO, doctor);
        await _repositoryManager.Doctor.UpdateDoctorAsync(doctor);

        return new ResponseMessage();
    }
}
