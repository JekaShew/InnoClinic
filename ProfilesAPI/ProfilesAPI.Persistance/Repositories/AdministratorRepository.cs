using Dapper;
using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;
using System.Text;

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
        var query =
            "Insert into Administrators " +
                "(Id, UserId, WorkStatusId, OfficeId, FirstName, LastName," +
                " SecondName, Address, WorkEmail, Phone, BirthDate, CareerStartDate, Photo, PhotoId) " +
            "Values (@Id, @UserId, @WorkStatusId, @OfficeId, @FirstName, @LastName, " +
                "@SecondName, @Address, @WorkEmail, @Phone, @BirthDate, @CareerStartDate, @Photo, @PhotoId) ";

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
        parameters.Add("Photo", administrator.Photo, System.Data.DbType.String);
        parameters.Add("PhotoId", administrator.PhotoId, System.Data.DbType.Guid);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task DeleteAdministratorByIdAsync(Guid administratorId)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Delete From Administrators " +
                "Where Administrators.Id = @AdministratorId ";
            await connection.ExecuteAsync(query, new { administratorId });
        }
    }

    public async Task<Administrator> GetAdministratorByIdAsync(Guid administratorId)
    {
        var query = "Select * From Administrators " +
            "Where Administrators.Id = @AdministratorId ";

        using (var connection = _profilesDBContext.Connection)
        {
            var administrator = await connection.QueryFirstOrDefaultAsync<Administrator>(query, new { administratorId});
            return administrator;
        }        
    }

    public async Task<ICollection<Administrator>> GetAllAdministratorsAsync(AdministratorParameters? administratorParameters)
    {
        var query = new StringBuilder(@"
            SELECT Administrators.Id, Administrators.UserId, Administrators.WorkStatusId, Administrators.OfficeId, 
                   Administrators.FirstName, Administrators.LastName, Administrators.SecondName, Administrators.Address,
                   Administrators.WorkEmail, Administrators.Phone, Administrators.BirthDate, Administrators.CareerStartDate, 
                   Administrators.Photo, Administrators.PhotoId
            FROM Administrators ");

        if(administratorParameters is null)
        {
            administratorParameters = new AdministratorParameters();
        }
        
        if (administratorParameters.Offices != null)
        {
            var officeList = string.Join(", ", administratorParameters.Offices.Select(id => $"'{id}'"));
            query.Append($@"
                WHERE Administrators.OfficeId IN ({officeList}) ");
        }

        if (administratorParameters.SearchString is not null && administratorParameters.SearchString.Length > 0)
        {           
            if(administratorParameters.Offices is null || administratorParameters.Offices.Count == 0)
            {
                query.Append($@"
            WHERE 
            CONCAT(Administrators.FirstName, ' ', Administrators.LastName, ' ', Administrators.SecondName) LIKE '%{administratorParameters.SearchString}%' ");
            }
            else
            {
                query.Append($@"
            AND
            CONCAT(Administrators.FirstName, ' ', Administrators.LastName, ' ', Administrators.SecondName) LIKE '%{administratorParameters.SearchString}%' ");      
            }
        }

        query.Append($@"
        ORDER BY Administrators.Id
        OFFSET {(administratorParameters.PageNumber - 1) * administratorParameters.PageSize} ROWS 
        FETCH NEXT {administratorParameters.PageSize} ROWS ONLY; ");
        string finalQuery = query.ToString();
        using (var connection = _profilesDBContext.Connection)
        {
            var administrators = await connection.QueryAsync<Administrator>(finalQuery);
            return administrators.ToList();
        }
    }

    public async Task<bool> IsProfileExists(Guid userId)
    {
        var query = "Select * From Administrators " +
            "Where Administrators.UserId = @UserId ";
        using (var connection = _profilesDBContext.Connection)
        {
            var administrator = await connection.QueryFirstOrDefaultAsync<Administrator>(query, new { userId });
            var result = administrator is null ? false : true;

            return result;
        }
    }

    public async Task UpdateAdministratorAsync(Guid administratorId, Administrator updatedAdministrator)
    {
        var query = "Update Administrators " +
                    "Set WorkStatusId = @WorkStatusId, OfficeId = @OfficeId, FirstName = @FirstName, " +
                        "LastName = @LastName, SecondName = @SecondName, Address = @Address, WorkEmail = @WorkEmail, " +
                        "Phone = @Phone , BirthDate = @BirthDate, CareerStartDate = @CareerStartDate, " +
                        "Photo = @Photo, PhotoId = @PhotoId " +
                    "Where Id = @AdministratorId ";

        var administratorParameters = new DynamicParameters();
        administratorParameters.Add("AdministratorId", administratorId, System.Data.DbType.Guid);
        administratorParameters.Add("WorkStatusId", updatedAdministrator.WorkStatusId, System.Data.DbType.Guid);
        administratorParameters.Add("OfficeId", updatedAdministrator.OfficeId, System.Data.DbType.Guid);
        administratorParameters.Add("FirstName", updatedAdministrator.FirstName, System.Data.DbType.String);
        administratorParameters.Add("LastName", updatedAdministrator.LastName, System.Data.DbType.String);
        administratorParameters.Add("SecondName", updatedAdministrator.SecondName, System.Data.DbType.String);
        administratorParameters.Add("Address", updatedAdministrator.Address, System.Data.DbType.String);
        administratorParameters.Add("WorkEmail", updatedAdministrator.WorkEmail, System.Data.DbType.String);
        administratorParameters.Add("Phone", updatedAdministrator.Phone, System.Data.DbType.String);
        administratorParameters.Add("BirthDate", updatedAdministrator.BirthDate, System.Data.DbType.DateTime);
        administratorParameters.Add("CareerStartDate", updatedAdministrator.CareerStartDate, System.Data.DbType.DateTime);
        administratorParameters.Add("Photo", updatedAdministrator.Photo, System.Data.DbType.String);
        administratorParameters.Add("PhotoId", updatedAdministrator.PhotoId, System.Data.DbType.Guid);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, administratorParameters);
        }
    }
}
