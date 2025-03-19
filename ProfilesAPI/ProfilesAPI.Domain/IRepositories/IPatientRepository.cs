using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Domain.IRepositories;

public interface IPatientRepository
{
    public Task AddPatientAsync(Patient patient);
    public Task UpdatePatientAsync(Guid patientId, Patient updatedPatient);
    public Task DeletePatientByIdAsync(Guid patientId);
    public Task<Patient> GetPatientByIdAsync(Guid patientId);
    public Task<ICollection<Patient>> GetAllPatientsAsync(PatientParameters? patientParameters);
    public Task<bool> IsProfileExists(Guid userId);
}
