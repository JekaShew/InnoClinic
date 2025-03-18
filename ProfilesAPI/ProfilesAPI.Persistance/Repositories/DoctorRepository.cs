using Dapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;
using System.Linq.Expressions;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ProfilesAPI.Persistance.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public DoctorRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task AddDoctorAsync(Doctor doctor)
    {
        //await _profilesDBContext.Connection.InsertAsync<Doctor>(doctor);
        var queryDoctor =
                    "Insert into Doctors " +
                        "(Id, UserId, WorkStatusId, OfficeId, FirstName, LastName," +
                        " SecondName, Address, WorkEmail, Phone, BirthDate, CareerStartDate, Photo, PhotoId)" +
                    "Values (@Id, @UserId, @WorkStatusId, @OfficeId, @FirstName, @LastName, " +
                        "@SecondName, @Address, @WorkEmail, @Phone, @BirthDate, @CareerStartDate, @Photo, @PhotoId)";

        var queryDoctorSpecializations =
                    "Insert into DoctorSpecializations " +
                        "(Id, DoctorId, SpecializationId, SpecialzationAchievementDate, Description)" +
                    "Values (@Id, @DoctorId, @SpecializationId, @SpecialzationAchievementDate, @Description)";

        var doctorParameters = new DynamicParameters();
        doctorParameters.Add("Id", Guid.NewGuid(), System.Data.DbType.Guid);
        doctorParameters.Add("UserId", doctor.UserId, System.Data.DbType.Guid);
        doctorParameters.Add("WorkStatusId", doctor.WorkStatusId, System.Data.DbType.Guid);
        doctorParameters.Add("OfficeId", doctor.OfficeId, System.Data.DbType.Guid);
        doctorParameters.Add("FirstName", doctor.FirstName, System.Data.DbType.String);
        doctorParameters.Add("LastName", doctor.LastName, System.Data.DbType.String);
        doctorParameters.Add("SecondName", doctor.SecondName, System.Data.DbType.String);
        doctorParameters.Add("Address", doctor.Address, System.Data.DbType.String);
        doctorParameters.Add("WorkEmail", doctor.WorkEmail, System.Data.DbType.String);
        doctorParameters.Add("Phone", doctor.Phone, System.Data.DbType.String);
        doctorParameters.Add("BirthDate", doctor.BirthDate, System.Data.DbType.DateTime);
        doctorParameters.Add("CareerStartDate", doctor.CareerStartDate, System.Data.DbType.DateTime);
        doctorParameters.Add("Photo", doctor.Photo, System.Data.DbType.String);
        doctorParameters.Add("PhotoId", doctor.PhotoId, System.Data.DbType.Guid);

        var doctorSpecializationsParametersList = new List<DynamicParameters>();
        foreach(var doctorSpecialization in doctor.DoctorSpecializations)
        {
            var doctorSpecializationsParameters = new DynamicParameters();
            doctorSpecializationsParameters.Add("Id", Guid.NewGuid(), System.Data.DbType.Guid);
            doctorSpecializationsParameters.Add("DoctorId", doctorParameters.Get<Guid>("Id"), System.Data.DbType.Guid);
            doctorSpecializationsParameters.Add("SpecializationId", doctorSpecialization.SpecializationId, System.Data.DbType.Guid);
            doctorSpecializationsParameters.Add("SpecialzationAchievementDate", doctorSpecialization.SpecialzationAchievementDate, System.Data.DbType.DateTime);
            doctorSpecializationsParameters.Add("Description", doctorSpecialization.Description, System.Data.DbType.String);

            doctorSpecializationsParametersList.Add(doctorSpecializationsParameters);
        }

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(queryDoctor, doctorParameters);
            foreach (var doctorSpecializationsParameters in doctorSpecializationsParametersList)
            {
                await connection.ExecuteAsync(queryDoctorSpecializations, doctorSpecializationsParameters);
            }
        }
    }

    public async Task DeleteDoctorByIdAsync(Guid doctorId)
    {
        //await _profilesDBContext.Connection.DeleteAsync<Doctor>(new Doctor { Id = doctorId });
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Delete From Doctors" +
                "Where Doctors.Id = @DoctorId";
            await connection.ExecuteAsync(query, new { doctorId });
        }
    }

    public async Task<ICollection<Doctor>> GetAllDoctorsAsync(DoctorParameters? doctorParameters)
    {
        //var query = "Select Doctors.Id, Doctors.UserId, Doctors.WorkStatusId, Doctors.OfficeId, " +
        //    "Doctors.FirstName, Doctors.LastName, Doctors.SecondName, Doctors.Address, Doctors.WorkEmail, " +
        //    "Doctors.Phone, Doctors.BirthDate, Doctors.CareerStartDate, Doctors.Photo, Doctors.PhotoId, " +
        //    "DoctorSpecializations.Id, DoctorSpecializations.DoctorId, DoctorSpecializations.SpecializationId, " +
        //    "DoctorSpecializations.SpecialzationAchievementDate, DoctorSpecializations.Description " +
        //    "From Doctors " +
        //    "inner join DoctorSpecializations on Doctors.Id = DoctorSpecializations.DoctorId " +
        //    "inner join Specializations on Specializations.Id = DoctorSpecializations.SpecializationId " +
        //    doctorParameters.Specializations is null ? " " :
        //        doctorParameters.Specializations.Count >= 1 ?
        //            "Where Doctors.Id in " +
        //            "(Select Distinct DoctorSpecializations.DoctorId " +
        //            "From DoctorSpecializations " +
        //            $"Where DoctorSpecializations.SpecializationId in {specializationList}) " : " " +
        //    $"Limit {doctorParameters.PageSize} Offset {(doctorParameters.PageNumber - 1) * doctorParameters.PageSize} ";

        var query = new StringBuilder(@"
            SELECT Doctors.Id, Doctors.UserId, Doctors.WorkStatusId, Doctors.OfficeId, 
                   Doctors.FirstName, Doctors.LastName, Doctors.SecondName, Doctors.Address, Doctors.WorkEmail, 
                   Doctors.Phone, Doctors.BirthDate, Doctors.CareerStartDate, Doctors.Photo, Doctors.PhotoId, 
                   DoctorSpecializations.Id, DoctorSpecializations.DoctorId, DoctorSpecializations.SpecializationId, 
                   DoctorSpecializations.SpecialzationAchievementDate, DoctorSpecializations.Description 
            FROM Doctors 
            INNER JOIN DoctorSpecializations ON Doctors.Id = DoctorSpecializations.DoctorId 
            INNER JOIN Specializations ON Specializations.Id = DoctorSpecializations.SpecializationId");

        var specializationList = "";
        var officeList = "";

        switch (doctorParameters)
        {
            case null:
                doctorParameters = new DoctorParameters();
                break;

            case { Specializations: { Count: > 0 }, Offices: { Count: <= 0} }:
                // specializations
                specializationList = string.Join(", ", doctorParameters.Specializations.Select(id => $"'{id}'"));
                query.Append($@"
                WHERE Doctors.Id IN (
                    SELECT DISTINCT DoctorSpecializations.DoctorId 
                    FROM DoctorSpecializations 
                    WHERE DoctorSpecializations.SpecializationId IN ({specializationList})
                )");
                break;

            case { Specializations: { Count: <= 0 }, Offices: { Count: > 0 } }:
                // offices  
                officeList = string.Join(", ", doctorParameters.Offices.Select(id => $"'{id}'"));                      
                query.Append($@"
                WHERE Doctors.OfficeId IN ({officeList}) ");
                break;

            case { Specializations: { Count: > 0 }, Offices: { Count: > 0 } }:
                // Specializations and Offices
                specializationList = string.Join(", ", doctorParameters.Specializations.Select(id => $"'{id}'"));
                officeList = string.Join(", ", doctorParameters.Offices.Select(id => $"'{id}'"));
                query.Append($@"
                WHERE Doctors.Id IN 
                (
                    SELECT DISTINCT DoctorSpecializations.DoctorId 
                    FROM DoctorSpecializations 
                    WHERE DoctorSpecializations.SpecializationId IN ({specializationList})
                ) AND
                Doctors.OfficeId IN ({officeList}) ");
                break;
        }
        if(doctorParameters.SearchString is not null && doctorParameters.SearchString.Length > 0)
        {
            if (
                doctorParameters.Specializations is null
                || doctorParameters.Specializations.Count == 0
                    &&
                doctorParameters.Offices is null
                || doctorParameters.Offices.Count == 0)
            {
                query.Append($@"
                WHERE 
                CONCAT(Doctors.FirstName, ' ', Doctors.LastName, ' ', Doctors.SecondName) LIKE '%{doctorParameters.SearchString}%' ");
            }
            else
            {
                query.Append($@"
                AND 
                CONCAT(Doctors.FirstName, ' ', Doctors.LastName, ' ', Doctors.SecondName) LIKE '%{doctorParameters.SearchString}%' ");
            }
        }

        query.Append($@"
        ORDER BY Doctors.Id
        OFFSET {(doctorParameters.PageNumber - 1) * doctorParameters.PageSize} ROWS 
        FETCH NEXT {doctorParameters.PageSize} ROWS ONLY; ");
        string finalQuery = query.ToString();
        using (var connection = _profilesDBContext.Connection)
        {
            var doctorDictionary = new Dictionary<Guid, Doctor>();
            var doctors = await connection.QueryAsync<Doctor, DoctorSpecialization, Doctor>(
                finalQuery, (doctor, doctorSpecialization) =>
                {
                    if (!doctorDictionary.TryGetValue(doctor.Id, out var currentDoctor))
                    {
                        currentDoctor = doctor;
                        currentDoctor.DoctorSpecializations = new List<DoctorSpecialization>();
                        doctorDictionary.Add(currentDoctor.Id, currentDoctor);
                    }

                    currentDoctor.DoctorSpecializations.Add(doctorSpecialization);

                    return currentDoctor;
                });

            return doctors.Distinct().ToList();
        }           
    }

    public async Task<Doctor> GetDoctorByIdAsync(Guid doctorId)
    {
        //var doctor = await _profilesDBContext.Connection.GetAsync<Doctor>(doctorId);
        var query = "Select * From Doctors " +
            "Where Doctors.Id = @DoctorId; " +
            "Select * From DoctorSpecializations " +
            "Where DoctorSpecializations.DoctorId = @DoctorId ";

        using (var connection = _profilesDBContext.Connection)
        using (var multi = await connection.QueryMultipleAsync(query, new { doctorId }))
        {
            var doctor = await multi.ReadFirstOrDefaultAsync<Doctor>();
            if(doctor is not null)
            {
                var doctorSpecializations = await multi.ReadAsync<DoctorSpecialization>();
                doctor.DoctorSpecializations = doctorSpecializations.ToList();
            }

            return doctor;
        }
    }

    public async Task UpdateDoctorAsync(Guid doctorId, Doctor updatedDoctor)
    {

        //await _profilesDBContext.Connection.UpdateAsync<Doctor>(updatedDoctor);
        var queryDoctor = "Update Doctors " +
                    "Set WorkStatusId = @WorkStatusId, OfficeId = @OfficeId, FirstName = @FirstName, " +
                        "LastName = @LastName, SecondName = @SecondName, Address = @Address, WorkEmail = @WorkEmail, " +
                        "Phone = @Phone , BirthDate = @BirthDate, CareerStartDate = @CareerStartDate, " +
                        "Photo = @Photo, PhotoId = @PhotoId " +
                    "Where Id = @DoctorId ";

        var doctorParameters = new DynamicParameters();
        doctorParameters.Add("DoctorId", doctorId, System.Data.DbType.Guid);
        doctorParameters.Add("WorkStatusId", updatedDoctor.WorkStatusId, System.Data.DbType.Guid);
        doctorParameters.Add("OfficeId", updatedDoctor.OfficeId, System.Data.DbType.Guid);
        doctorParameters.Add("FirstName", updatedDoctor.FirstName, System.Data.DbType.String);
        doctorParameters.Add("LastName", updatedDoctor.LastName, System.Data.DbType.String);
        doctorParameters.Add("SecondName", updatedDoctor.SecondName, System.Data.DbType.String);
        doctorParameters.Add("Address", updatedDoctor.Address, System.Data.DbType.String);
        doctorParameters.Add("WorkEmail", updatedDoctor.WorkEmail, System.Data.DbType.String);
        doctorParameters.Add("Phone", updatedDoctor.Phone, System.Data.DbType.String);
        doctorParameters.Add("BirthDate", updatedDoctor.BirthDate, System.Data.DbType.DateTime);
        doctorParameters.Add("CareerStartDate", updatedDoctor.CareerStartDate, System.Data.DbType.DateTime);
        doctorParameters.Add("Photo", updatedDoctor.Photo, System.Data.DbType.String);
        doctorParameters.Add("PhotoId", updatedDoctor.PhotoId, System.Data.DbType.Guid);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(queryDoctor, doctorParameters);
        }
    }

    public Task<ICollection<Doctor>> GetDoctorsByExpression(Expression<Func<Doctor, bool>> expression)
    {
        throw new Exception("Not Implemented");
    }

    public async Task<ICollection<Doctor>> GetFilteredDoctors(ICollection<Guid> specializtions, ICollection<string> offices, string QueryString)
    {
        //var query1 = "Select Doctors.FirstName, Doctors.LastName, Doctors.SecondName," +
        //    "Doctors.Adress, Doctors.Phone, Doctors.BirthDate, Doctors.Photo," +
        //    "Doctors.Workemail, Doctors.CareerStartDate, Doctors.WorkStatusId, Doctors.OfficeId " +
        //    "From DoctorSpecializations  inner join Doctors on Doctors.Id = DoctorSpecializations,DoctorId" +
        //    "inner join WorkStatuses on WorkStatuses.Id = Doctors.WorkStatusId" +
        //    "inner join Specializations on Specialization.Id = DoctorSpecializations.SpecializationId" +
        //    "Where DoctorSpecializations.SpecializationId in @Specializations" +
        //    "and "
        //var query = "SELECT * FROM Doctors WHERE " +
        //    " " +
        //    " ";
        //using (var connection = _profilesDBContext.Connection)
        //{
        //    var doctors = await connection.QueryAsync<Doctor>(query);

        //    return doctors.ToList();
        //}

        throw new Exception("Not Implemented");
    }

    public async Task DeleteSelectedDoctorSpecializationsByDoctorIdAsync(Guid doctorId)
    {
        var query = "Delete From DoctorSpecializations " +
                         "Where DoctorSpecializations.DoctorId = @DoctorId ";

        var doctorParameters = new DynamicParameters();
        doctorParameters.Add("DoctorId", doctorId, System.Data.DbType.Guid);
       

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, doctorParameters);
        }
    }

    public async Task AddSelectedDoctorSpecializationAsync(Guid doctorId, ICollection<DoctorSpecialization> doctorSpecializationsToAdd)
    {
        var query = "Insert into DoctorSpecializations " +
                        "(Id, DoctorId, SpecializationId, SpecialzationAchievementDate, Description) " +
                    "Values (@Id, @DoctorId, @SpecializationId, @SpecialzationAchievementDate, @Description) ";

        var doctorSpecializationsParametersList = new List<DynamicParameters>();
        foreach (var doctorSpecialization in doctorSpecializationsToAdd)
        {
            var doctorSpecializationsParameters = new DynamicParameters();
            doctorSpecializationsParameters.Add("Id", Guid.NewGuid(), System.Data.DbType.Guid);
            doctorSpecializationsParameters.Add("DoctorId", doctorId, System.Data.DbType.Guid);
            doctorSpecializationsParameters.Add("SpecializationId", doctorSpecialization.SpecializationId, System.Data.DbType.Guid);
            doctorSpecializationsParameters.Add("SpecialzationAchievementDate", doctorSpecialization.SpecialzationAchievementDate, System.Data.DbType.DateTime);
            doctorSpecializationsParameters.Add("Description", doctorSpecialization.Description, System.Data.DbType.String);

            doctorSpecializationsParametersList.Add(doctorSpecializationsParameters);
        }

        using (var connection = _profilesDBContext.Connection)
        {
            foreach (var doctorSpecializationsParameters in doctorSpecializationsParametersList)
            {
                await connection.ExecuteAsync(query, doctorSpecializationsParameters);
            }
        }
    }

    public async Task<bool> IsProfileExists(Guid userId)
    {
        var query = "Select * From Doctors " +
            "Where Doctors.UserId = @UserId ";
        using (var connection = _profilesDBContext.Connection)
        {
            var doctor = await connection.QueryFirstOrDefaultAsync<Doctor>(query, new { userId });
            var result = doctor is null ? false : true;
            
            return result;
        }
    }
}
