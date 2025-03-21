using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Domain.IRepositories;

public interface IPatientRepository
{
    public Task<Guid> CreateAsync(Patient patient);
    public Task UpdateAsync(Guid patientId, Patient updatedPatient);
    public Task DeleteByIdAsync(Guid patientId);
    public Task<Patient> GetByIdAsync(Guid patientId);
    public Task<ICollection<Patient>> GetAllAsync(PatientParameters? patientParameters);
    public Task<bool> IsProfileExists(Guid userId);
}
