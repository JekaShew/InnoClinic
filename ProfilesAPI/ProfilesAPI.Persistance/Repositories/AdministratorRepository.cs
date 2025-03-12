using Dapper;
using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;
using System.Numerics;

namespace ProfilesAPI.Persistance.Repositories;

public class AdministratorRepository : IAdministratorRepository
{
    private readonly ProfilesDBContext _profilesDBContext;
    public AdministratorRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task AddAdministratorAsync(Administrator administrator)
    {
        //await _profilesDBContext.Connection.InsertAsync<Administrator>(administrator);
        var query =
                   "Insert into Administrators " +
                       "(Id, UserId, WorkStatusId, OfficeId, FirstName, LastName," +
                       " SecondName, Address, WorkEmail, Phone, BirthDate, CareerStartDate, Photo)" +
                   "Values (@Id, @UserId, @WorkStatusId, @OfficeId, @FirstName, @LastName, " +
                       "@SecondName, @Address, @WorkEmail, @Phone, @BirthDate, @CareerStartDate, @Photo)";

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.NewGuid(), System.Data.DbType.Guid);
        parameters.Add("UserId", administrator.UserId, System.Data.DbType.Guid);
        parameters.Add("WorkStatusId", administrator.WorkStatusId, System.Data.DbType.Guid);
        parameters.Add("OfficeId", administrator.OfficeId, System.Data.DbType.Guid);
        parameters.Add("FirstName", administrator.FirstName, System.Data.DbType.String);
        parameters.Add("LastName", administrator.LastName, System.Data.DbType.String);
        parameters.Add("SecondName", administrator.SecondName, System.Data.DbType.String);
        parameters.Add("Address", administrator.Address, System.Data.DbType.String);
        parameters.Add("WorkEmail", administrator.WorkEmail, System.Data.DbType.String);
        parameters.Add("Phone", administrator.Phone, System.Data.DbType.String);
        parameters.Add("BirthDate", administrator.BirthDate, System.Data.DbType.DateTime);
        parameters.Add("CareerStartDate", administrator.CareerStartDate, System.Data.DbType.DateTime);

        parameters.Add("Photo", administrator.Photo, System.Data.DbType.Guid);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task DeleteAdministratorByIdAsync(Guid administratorId)
    {
        await _profilesDBContext.Connection.DeleteAsync<Administrator>(new Administrator { UserId = administratorId });
    }

    public async Task<Administrator> GetAdministratorByIdAsync(Guid administratorId)
    {
        var administrator = await _profilesDBContext.Connection.GetAsync<Administrator>(administratorId);

        return administrator;
    }

    public async Task<ICollection<Administrator>> GetAllAdministratorsAsync()
    {
        var administrators = await _profilesDBContext.Connection.GetAllAsync<Administrator>();

        return administrators.ToList();
    }

    public async Task UpdateAdministratorAsync(Administrator updatedAdministrator)
    {
        await _profilesDBContext.Connection.UpdateAsync<Administrator>(updatedAdministrator);
    }
}
