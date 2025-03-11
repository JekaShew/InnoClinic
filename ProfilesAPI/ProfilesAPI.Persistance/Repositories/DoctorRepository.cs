using Dapper;
using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;
using System.Linq.Expressions;

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
                        " SecondName, Address, WorkEmail, Phone, BirthDate, CareerStartDate, Photo" +
                    "Values (@Id, @UserId, @WorkStatusId, @OfficeId, @FirstName, @LastName, " +
                        "@SecondName, @Address, @WorkEmail, @Phone, @BirthDate, @CareerStartDate, @Photo)";

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
        doctorParameters.Add("Photo", doctor.Photo, System.Data.DbType.Guid);

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
        var doctors = await _profilesDBContext.Connection.GetAllAsync<Doctor>();

        return doctors.ToList();
    }

    public async Task<Doctor> GetDoctorByIdAsync(Guid doctorId)
    {
        var doctor = await _profilesDBContext.Connection.GetAsync<Doctor>(doctorId);

        return doctor;
    }

    public async Task UpdateDoctorAsync(Doctor updatedDoctor)
    {
        await _profilesDBContext.Connection.UpdateAsync<Doctor>(updatedDoctor);
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
}
