using Dapper;
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
        //await _profilesDBContext.Connection.InsertAsync<Patient>(patient);
        var query = 
            "Insert into Patients " +
                "(Id, UserId, FirstName, LastName," +
                " SecondName, Address, Phone, BirthDate, Photo" +
            "Values (@Id, @UserId, @FirstName, @LastName, " +
                "@SecondName, @Address, @Phone, @BirthDate, @Photo)";

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.NewGuid(), System.Data.DbType.Guid);
        parameters.Add("UserId", patient.UserId, System.Data.DbType.Guid);
        parameters.Add("FirstName", patient.FirstName, System.Data.DbType.String);
        parameters.Add("LastName", patient.LastName, System.Data.DbType.String);
        parameters.Add("SecondName", patient.SecondName, System.Data.DbType.String);
        parameters.Add("Address", patient.Address, System.Data.DbType.String);
        parameters.Add("Phone", patient.Phone, System.Data.DbType.String);
        parameters.Add("BirthDate", patient.BirthDate, System.Data.DbType.DateTime);
        parameters.Add("Photo", patient.Photo, System.Data.DbType.Guid);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, parameters);
        }

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
