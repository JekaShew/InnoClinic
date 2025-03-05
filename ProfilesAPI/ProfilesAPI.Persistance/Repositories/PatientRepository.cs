using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public PatientRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task AddPatientAsync(Patient patient)
    {
        await _profilesDBContext.Connection.InsertAsync<Patient>(patient);
    }

    public async Task DeletePatientByIdAsync(Guid patientId)
    {
        await _profilesDBContext.Connection.DeleteAsync<Patient>(new Patient { UserId = patientId });
    }

    public async Task<ICollection<Patient>> GetAllPatientsAsync()
    {
        var patients = await _profilesDBContext.Connection.GetAllAsync<Patient>();

        return patients.ToList();
    }

    public async Task<Patient> GetPatientByIdAsync(Guid patientId)
    {
        var patient = await _profilesDBContext.Connection.GetAsync<Patient>(patientId);

        return patient;
    }

    public async Task UpdatePatientAsync(Patient updatedPatient)
    {
        await _profilesDBContext.Connection.UpdateAsync<Patient>(updatedPatient);
    }
}
