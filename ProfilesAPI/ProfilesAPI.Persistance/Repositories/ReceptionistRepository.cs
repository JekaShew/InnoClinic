using Dapper;
using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;
using System.Numerics;

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
                       "(Id, UserId, WorkStatusId, OfficeId, FirstName, LastName," +
                       " SecondName, Address, WorkEmail, Phone, BirthDate, CareerStartDate, Photo)" +
                   "Values (@Id, @UserId, @WorkStatusId, @OfficeId, @FirstName, @LastName, " +
                       "@SecondName, @Address, @WorkEmail, @Phone, @BirthDate, @CareerStartDate, @Photo)";

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
        var receptionists = await _profilesDBContext.Connection.GetAllAsync<Receptionist>();

        return receptionists.ToList();
    }

    public async Task<Receptionist> GetReceptionistByIdAsync(Guid receptionistId)
    {
        var receptionist = await _profilesDBContext.Connection.GetAsync<Receptionist>(receptionistId);

        return receptionist;
    }

    public async Task UpdateReceptionistAsync(Receptionist updatedReceptionist)
    {
        await _profilesDBContext.Connection.UpdateAsync<Receptionist>(updatedReceptionist);
    }
}
