using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IPatientService
{
    public Task AddPatientAsync(PatientForCreateDTO patientForCreateDTO);
    public Task UpdatePatientAsync(Guid patientId, PatientForUpdateDTO patientForUpdateDTO);
    public Task DeletePatientByIdAsync(Guid patientId);
    public Task<PatientInfoDTO> GetPatientByIdAsync(Guid patientId);
    public Task<ICollection<PatientTableInfoDTO>> GetAllPatientsAsync();
}
