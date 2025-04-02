using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Shared.DTOs.PatientDTOs;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IPatientService
{
    public Task<ResponseMessage<PatientInfoDTO>> CreatePatientAsync(PatientForCreateDTO patientForCreateDTO);
    public Task<ResponseMessage<PatientInfoDTO>> UpdatePatientAsync(Guid patientId, PatientForUpdateDTO patientForUpdateDTO);
    public Task<ResponseMessage> DeletePatientByIdAsync(Guid patientId);
    public Task<ResponseMessage<PatientInfoDTO>> GetPatientByIdAsync(Guid patientId);
    public Task<ResponseMessage<ICollection<PatientTableInfoDTO>>> GetAllPatientsAsync(PatientParameters? patientParameters);
}
