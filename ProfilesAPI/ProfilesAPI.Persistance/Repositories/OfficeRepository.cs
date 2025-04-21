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
                "RegistryPhoneNumber, IsActive, IsDelete) " +
             "Values (@Id, @City, @Street, @HouseNumber, @OfficeNumber, @RegistryPhoneNumber, " +
                "@IsActive, @IsDelete) ";

        var parameters = new DynamicParameters();
        parameters.Add("Id", office.Id, System.Data.DbType.Guid);
        parameters.Add("City", office.City, System.Data.DbType.String);
        parameters.Add("Street", office.Street, System.Data.DbType.String);
        parameters.Add("HouseNumber", office.HouseNumber, System.Data.DbType.String);
        parameters.Add("OfficeNumber", office.OfficeNumber, System.Data.DbType.String);
        parameters.Add("RegistryPhoneNumber", office.RegistryPhoneNumber, System.Data.DbType.String);
        parameters.Add("IsActive", office.IsActive, System.Data.DbType.Boolean);
        parameters.Add("IsDelete", office.IsDelete, System.Data.DbType.Boolean);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task<Office> GetByIdAsync(Guid officeId)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Select * From Offices " +
                "Where Offices.Id = @OfficeId AND IsDelete = 0 ";
            var office = await connection.QueryFirstOrDefaultAsync<Office>(query, new { officeId });

            return office;
        }
    }

    public async Task UpdateAsync(Guid officeId, Office updatedOffice)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Update Offices " +
                "Set City = @City, Street = @Street, HouseNumber = @HouseNumber, " +
                "OfficeNumber = @OfficeNumber, RegistryPhoneNumber = @RegistryPhoneNumber, " +
                "IsActive = @IsActive, IsDelete = @IsDelete" +
                "Where Offices.Id = @OfficeId ";
            var parameters = new DynamicParameters();
            parameters.Add("OfficeId", officeId, System.Data.DbType.Guid);
            parameters.Add("City", updatedOffice.City, System.Data.DbType.String);
            parameters.Add("Street", updatedOffice.Street, System.Data.DbType.String);
            parameters.Add("HouseNumber", updatedOffice.HouseNumber, System.Data.DbType.String);
            parameters.Add("OfficeNumber", updatedOffice.OfficeNumber, System.Data.DbType.String);
            parameters.Add("RegistryPhoneNumber", updatedOffice.RegistryPhoneNumber, System.Data.DbType.String);
            parameters.Add("IsActive", updatedOffice.IsActive, System.Data.DbType.Boolean);
            parameters.Add("IsDelete", updatedOffice.IsDelete, System.Data.DbType.Boolean);
           
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task DeleteAsync(Office office)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Delete From Offices " +
                "Where Offices.Id = @OfficeId ";
            await connection.ExecuteAsync(query, new { office.Id });
        }
    }
    public async Task SoftDeleteAsync(Office office)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Update From Offices " +
                "Where Offices.Id = @OfficeId " +
                "SET IsDelete = @IsDelete ";

            var parameters = new DynamicParameters();
            parameters.Add("OfficeId", office.Id, System.Data.DbType.Guid);
            parameters.Add("IsDelete", true, System.Data.DbType.Boolean);
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
