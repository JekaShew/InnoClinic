using Dapper;
using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;
using System.Linq.Expressions;
using System.Numerics;

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
        // add check if Exists
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
        await _profilesDBContext.Connection.DeleteAsync<Doctor>(new Doctor { UserId = doctorId });
    }

    public async Task<ICollection<Doctor>> GetAllDoctorsAsync()
    {
        //var doctors = await _profilesDBContext.Connection.GetAllAsync<Doctor>();
        var query = "Select Doctors.Id, Doctors.UserId, Doctors.WorkStatusId, Doctors.OfficeId, " +
            "Doctors.FirstName, Doctors.LastName, Doctors.SecondName, Doctors.Address, Doctors.WorkEmail, " +
            "Doctors.Phone, Doctors.BirthDate, Doctors.CareerStartDate, Doctors.Photo, Doctors.PhotoId, " +
            "DoctorSpecializations.Id, DoctorSpecializations.DoctorId, DoctorSpecializations.SpecializationId, " +
            "DoctorSpecializations.SpecialzationAchievementDate, DoctorSpecializations.Description " +
            "From Doctors " +
            "inner join DoctorSpecializations on Doctors.Id = DoctorSpecializations.DoctorId " +
            "inner join Specializations on Specializations.Id = DoctorSpecializations.SpecializationId";

        using (var connection = _profilesDBContext.Connection)
        {
            var doctorDictionary = new Dictionary<Guid, Doctor>();
            var doctors = await connection.QueryAsync<Doctor, DoctorSpecialization, Doctor>(
                query, (doctor, doctorSpecialization) =>
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
}
