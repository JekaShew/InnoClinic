using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IPatientService
{
    public Task<ResponseMessage> AddPatientAsync(PatientForCreateDTO patientForCreateDTO);
    public Task<ResponseMessage> UpdatePatientAsync(Guid patientId, PatientForUpdateDTO patientForUpdateDTO);
    public Task<ResponseMessage> DeletePatientByIdAsync(Guid patientId);
    public Task<ResponseMessage<PatientInfoDTO>> GetPatientByIdAsync(Guid patientId);
    public Task<ResponseMessage<ICollection<PatientTableInfoDTO>>> GetAllPatientsAsync();
}
