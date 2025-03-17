using Dapper;
using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class ReceptionistRepository : IReceptionistRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public ReceptionistRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task AddReceptionistAsync(Receptionist receptionist)
    {
        //await _profilesDBContext.Connection.InsertAsync<Receptionist>(receptionist);
        var query =
                   "Insert into Receptionists " +
                       "(Id, UserId, WorkStatusId, OfficeId, FirstName, LastName, " +
                       "SecondName, Address, WorkEmail, Phone, BirthDate, CareerStartDate, Photo) " +
                   "Values (@Id, @UserId, @WorkStatusId, @OfficeId, @FirstName, @LastName, " +
                       "@SecondName, @Address, @WorkEmail, @Phone, @BirthDate, @CareerStartDate, @Photo) ";

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.NewGuid(), System.Data.DbType.Guid);
        parameters.Add("UserId", receptionist.UserId, System.Data.DbType.Guid);
        parameters.Add("WorkStatusId", receptionist.WorkStatusId, System.Data.DbType.Guid);
        parameters.Add("OfficeId", receptionist.OfficeId, System.Data.DbType.Guid);
        parameters.Add("FirstName", receptionist.FirstName, System.Data.DbType.String);
        parameters.Add("LastName", receptionist.LastName, System.Data.DbType.String);
        parameters.Add("SecondName", receptionist.SecondName, System.Data.DbType.String);
        parameters.Add("Address", receptionist.Address, System.Data.DbType.String);
        parameters.Add("WorkEmail", receptionist.WorkEmail, System.Data.DbType.String);
        parameters.Add("Phone", receptionist.Phone, System.Data.DbType.String);
        parameters.Add("BirthDate", receptionist.BirthDate, System.Data.DbType.DateTime);
        parameters.Add("CareerStartDate", receptionist.CareerStartDate, System.Data.DbType.DateTime);

        parameters.Add("Photo", receptionist.Photo, System.Data.DbType.Guid);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task DeleteReceptionistByIdAsync(Guid receptionistId)
    {
        await _profilesDBContext.Connection.DeleteAsync<Receptionist>(new Receptionist { UserId = receptionistId });
    }

    public async Task<ICollection<Receptionist>> GetAllReceptionistsAsync()
    {
        //var receptionists = await _profilesDBContext.Connection.GetAllAsync<Receptionist>();
        var query = "Select Receptionists.Id, Receptionists.UserId, Receptionists.WorkStatusId, Receptionists.OfficeId, " +
            "Receptionists.FirstName, Receptionists.LastName, DocReceptioniststors.SecondName, Receptionists.Address, Receptionists.WorkEmail, " +
            "Receptionists.Phone, Receptionists.BirthDate, Receptionists.CareerStartDate, Receptionists.Photo, Receptionists.PhotoId, " +
            "From Receptionists ";

        using (var connection = _profilesDBContext.Connection)
        {
            var receptionists = await connection.QueryAsync<Receptionist>(query);
            return receptionists.ToList();
        }
    }

    public async Task<Receptionist> GetReceptionistByIdAsync(Guid receptionistId)
    {
        //var receptionist = await _profilesDBContext.Connection.GetAsync<Receptionist>(receptionistId);
        var query = "Select * From Receptionists " +
            "Where Receptionists.Id = @ReceptionistId ";

        using (var connection = _profilesDBContext.Connection)
        {
            var receptionist = await connection.QueryFirstOrDefaultAsync<Receptionist>(query, new { receptionistId});
            return receptionist;
        }        
    }

    public async Task UpdateReceptionistAsync(Guid receptionistId, Receptionist updatedReceptionist)
    {
        //await _profilesDBContext.Connection.UpdateAsync<Receptionist>(updatedReceptionist);
        var query = "Update Receptionists " +
                    "Set WorkStatusId = @WorkStatusId, OfficeId = @OfficeId, FirstName = @FirstName, " +
                        "LastName = @LastName, SecondName = @SecondName, Address = @Address, WorkEmail = @WorkEmail, " +
                        "Phone = @Phone , BirthDate = @BirthDate, CareerStartDate = @CareerStartDate, " +
                        "Photo = @Photo, PhotoId = @PhotoId " +
                    "Where Id = @ReceptionistId ";

        var receptionistParameters = new DynamicParameters();
        receptionistParameters.Add("ReceptionistId", receptionistId, System.Data.DbType.Guid);
        receptionistParameters.Add("WorkStatusId", updatedReceptionist.WorkStatusId, System.Data.DbType.Guid);
        receptionistParameters.Add("OfficeId", updatedReceptionist.OfficeId, System.Data.DbType.Guid);
        receptionistParameters.Add("FirstName", updatedReceptionist.FirstName, System.Data.DbType.String);
        receptionistParameters.Add("LastName", updatedReceptionist.LastName, System.Data.DbType.String);
        receptionistParameters.Add("SecondName", updatedReceptionist.SecondName, System.Data.DbType.String);
        receptionistParameters.Add("Address", updatedReceptionist.Address, System.Data.DbType.String);
        receptionistParameters.Add("WorkEmail", updatedReceptionist.WorkEmail, System.Data.DbType.String);
        receptionistParameters.Add("Phone", updatedReceptionist.Phone, System.Data.DbType.String);
        receptionistParameters.Add("BirthDate", updatedReceptionist.BirthDate, System.Data.DbType.DateTime);
        receptionistParameters.Add("CareerStartDate", updatedReceptionist.CareerStartDate, System.Data.DbType.DateTime);
        receptionistParameters.Add("Photo", updatedReceptionist.Photo, System.Data.DbType.String);
        receptionistParameters.Add("PhotoId", updatedReceptionist.PhotoId, System.Data.DbType.Guid);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, receptionistParameters);
        }
    }
}
