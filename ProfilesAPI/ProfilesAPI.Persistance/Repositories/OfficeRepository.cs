using Dapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class OfficeRepository : IOfficeRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public OfficeRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task CreateAsync(Office office)
    {
        var query = "Insert into Offices (Id, City, Street, HouseNumber, OfficeNumber, " +
                "RegistryPhoneNumber, IsActive) " +
             "Values (@Id, @City, @Street, @HouseNumber, @OfficeNumber, @RegistryNumber, " +
                "@IsActive) ";

        var parameters = new DynamicParameters();
        parameters.Add("Id", office.Id, System.Data.DbType.String);
        parameters.Add("City", office.City, System.Data.DbType.String);
        parameters.Add("Street", office.Street, System.Data.DbType.String);
        parameters.Add("HouseNumber", office.HouseNumber, System.Data.DbType.String);
        parameters.Add("OfficeNumber", office.OfficeNumber, System.Data.DbType.String);
        parameters.Add("RegistryPhoneNumber", office.RegistryPhoneNumber, System.Data.DbType.String);
        parameters.Add("IsActive", office.IsActive, System.Data.DbType.Boolean);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task<Office> GetByIdAsync(string officeId)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Select * From Offices " +
                "Where Offices.Id = @OfficeId";
            var office = await connection.QueryFirstOrDefaultAsync<Office>(query, new { officeId });

            return office;
        }
    }

    public async Task UpdateAsync(string officeId, Office updatedOffice)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Update Offices " +
                "Set City = @City, Street = @Street, HouseNumber = @HouseNumber, " +
                "OfficeNumber = @OfficeNumber, RegistryPhoneNumber = @RegistryPhoneNumber, " +
                "IsActive = @IsActive " +
                "Where Offices.Id = @OfficeId ";
            var parameters = new DynamicParameters();
            parameters.Add("OfficeId", officeId, System.Data.DbType.Guid);
            parameters.Add("City", updatedOffice.City, System.Data.DbType.String);
            parameters.Add("Street", updatedOffice.Street, System.Data.DbType.String);
            parameters.Add("HouseNumber", updatedOffice.HouseNumber, System.Data.DbType.String);
            parameters.Add("OfficeNumber", updatedOffice.OfficeNumber, System.Data.DbType.String);
            parameters.Add("RegistryPhoneNumber", updatedOffice.RegistryPhoneNumber, System.Data.DbType.String);
            parameters.Add("IsActive", updatedOffice.IsActive, System.Data.DbType.Boolean);
           
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
