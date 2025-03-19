using Dapper;
using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;
using ProfilesAPI.Shared.DTOs.PatientDTOs;
using System.Text;

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
        var query = 
            "Insert into Patients " +
                "(Id, UserId, FirstName, LastName," +
                " SecondName, Address, Phone, BirthDate, Photo, PhotoId) " +
            "Values (@Id, @UserId, @FirstName, @LastName, " +
                "@SecondName, @Address, @Phone, @BirthDate, @Photo, @PhotoId) ";

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.NewGuid(), System.Data.DbType.Guid);
        parameters.Add("UserId", patient.UserId, System.Data.DbType.Guid);
        parameters.Add("FirstName", patient.FirstName, System.Data.DbType.String);
        parameters.Add("LastName", patient.LastName, System.Data.DbType.String);
        parameters.Add("SecondName", patient.SecondName, System.Data.DbType.String);
        parameters.Add("Address", patient.Address, System.Data.DbType.String);
        parameters.Add("Phone", patient.Phone, System.Data.DbType.String);
        parameters.Add("BirthDate", patient.BirthDate, System.Data.DbType.DateTime);
        parameters.Add("Photo", patient.Photo, System.Data.DbType.String);
        parameters.Add("PhotoId", patient.PhotoId, System.Data.DbType.Guid);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task DeletePatientByIdAsync(Guid patientId)
    {
        using(var connection  = _profilesDBContext.Connection)
        {
            var query = "Delete From Patients " +
                "Where Patients.Id = @PatientId ";
            await connection.ExecuteAsync(query, new { patientId });
        }
    }

    public async Task<ICollection<Patient>> GetAllPatientsAsync(PatientParameters? patientParameters)
    {
        var query = new StringBuilder(@"
            SELECT Patients.Id, Patients.UserId, Patients.FirstName, Patients.LastName, 
                Patients.SecondName, Patients.Address, Patients.Phone, Patients.BirthDate, 
                Patients.Photo, Patients.PhotoId            
            FROM Patients " );

        if(patientParameters is null)
        {
            patientParameters = new PatientParameters();
        }

        if (patientParameters.SearchString is not null && patientParameters.SearchString.Length > 0)
        {
            query.Append($@"
            WHERE 
            CONCAT(Patients.FirstName, ' ', Patients.LastName, ' ', Patients.SecondName) LIKE '%{patientParameters.SearchString}%' ");
        }

        query.Append($@"
        ORDER BY Patients.Id
        OFFSET 
        {(patientParameters.PageNumber - 1) * patientParameters.PageSize} ROWS 
        FETCH NEXT {patientParameters.PageSize} ROWS ONLY; ");
        string finalQuery = query.ToString();
        using (var connection = _profilesDBContext.Connection)
        {
            var patients = await connection.QueryAsync<Patient>(finalQuery);

            return patients.Distinct().ToList();
        }
    }

    public async Task<Patient> GetPatientByIdAsync(Guid patientId)
    {
        var query = "Select * From Patients " +
            "Where Patients.Id = @PatientId ";

        using (var connection = _profilesDBContext.Connection)
        {
            var patient = await connection.QueryFirstOrDefaultAsync<Patient>(query, new { patientId });  
            return patient;
        }
    }

    public async Task<bool> IsProfileExists(Guid userId)
    {
        var query = "Select * From Patients " +
            "Where Patients.UserId = @UserId ";
        using (var connection = _profilesDBContext.Connection)
        {
            var patient = await connection.QueryFirstOrDefaultAsync<Patient>(query, new { userId });
            var result = patient is null ? false : true;

            return result;
        }
    }

    public async Task UpdatePatientAsync(Guid patientId, Patient updatedPatient)
    {
        var query = "Update Patients " +
                   "Set FirstName = @FirstName, LastName = @LastName, SecondName = @SecondName, " +
                       " Address = @Address, Phone = @Phone , BirthDate = @BirthDate, " +
                       "Photo = @Photo, PhotoId = @PhotoId " +
                   "Where Id = @PatientId ";

        var patientParameters = new DynamicParameters();
        patientParameters.Add("PatientId", patientId, System.Data.DbType.Guid);
        patientParameters.Add("FirstName", updatedPatient.FirstName, System.Data.DbType.String);
        patientParameters.Add("LastName", updatedPatient.LastName, System.Data.DbType.String);
        patientParameters.Add("SecondName", updatedPatient.SecondName, System.Data.DbType.String);
        patientParameters.Add("Address", updatedPatient.Address, System.Data.DbType.String);
        patientParameters.Add("Phone", updatedPatient.Phone, System.Data.DbType.String);
        patientParameters.Add("BirthDate", updatedPatient.BirthDate, System.Data.DbType.DateTime);
        patientParameters.Add("Photo", updatedPatient.Photo, System.Data.DbType.String);
        patientParameters.Add("PhotoId", updatedPatient.PhotoId, System.Data.DbType.Guid);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, patientParameters);
        }
    }
}
