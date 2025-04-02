using Dapper;
using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;
using System.Linq;
using System.Text;

namespace ProfilesAPI.Persistance.Repositories;

public class ReceptionistRepository : IReceptionistRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public ReceptionistRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task CreateAsync(Receptionist receptionist)
    {
        var query =
            "Insert into Receptionists " +
                "(Id, UserId, WorkStatusId, OfficeId, FirstName, LastName, " +
                "SecondName, Address, WorkEmail, Phone, BirthDate, CareerStartDate, Photo, PhotoId) " +
            "Values (@Id, @UserId, @WorkStatusId, @OfficeId, @FirstName, @LastName, " +
                "@SecondName, @Address, @WorkEmail, @Phone, @BirthDate, @CareerStartDate, @Photo, @PhotoId) ";

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.NewGuid(), System.Data.DbType.Guid);
        parameters.Add("UserId", receptionist.UserId, System.Data.DbType.Guid);
        parameters.Add("WorkStatusId", receptionist.WorkStatusId, System.Data.DbType.Guid);
        parameters.Add("OfficeId", receptionist.OfficeId, System.Data.DbType.String);
        parameters.Add("FirstName", receptionist.FirstName, System.Data.DbType.String);
        parameters.Add("LastName", receptionist.LastName, System.Data.DbType.String);
        parameters.Add("SecondName", receptionist.SecondName, System.Data.DbType.String);
        parameters.Add("Address", receptionist.Address, System.Data.DbType.String);
        parameters.Add("WorkEmail", receptionist.WorkEmail, System.Data.DbType.String);
        parameters.Add("Phone", receptionist.Phone, System.Data.DbType.String);
        parameters.Add("BirthDate", receptionist.BirthDate, System.Data.DbType.DateTime);
        parameters.Add("CareerStartDate", receptionist.CareerStartDate, System.Data.DbType.DateTime);
        parameters.Add("Photo", receptionist.Photo, System.Data.DbType.String);
        parameters.Add("PhotoId", receptionist.PhotoId, System.Data.DbType.Guid);

        using (var connection = _profilesDBContext.Connection)
        {
            await connection.ExecuteAsync(query, parameters);   
        }
    }

    public async Task DeleteByIdAsync(Guid receptionistId)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Delete From Receptionists " +
                "Where Receptionists.Id = @ReceptionistId ";
            await connection.ExecuteAsync(query, new { receptionistId });
        }
    }

    public async Task<ICollection<Receptionist>> GetAllAsync(ReceptionistParameters? receptionistPrameters)
    {
        var query = new StringBuilder(@"
            SELECT Receptionists.Id, Receptionists.UserId, Receptionists.WorkStatusId, Receptionists.OfficeId,
                  Receptionists.FirstName, Receptionists.LastName, Receptionists.SecondName, 
                  Receptionists.Address, Receptionists.WorkEmail, Receptionists.Phone, Receptionists.BirthDate,
                  Receptionists.CareerStartDate, Receptionists.Photo, Receptionists.PhotoId
            FROM Receptionists ");
        
        if (receptionistPrameters is null)
        {
            receptionistPrameters = new ReceptionistParameters();
        }

        if (receptionistPrameters.Offices != null)
        {
            var officeList = string.Join(", ", receptionistPrameters.Offices.Select(id => $"'{id}'"));
            query.Append($@"
                WHERE Receptionists.OfficeId IN ({officeList}) ");
        }

        if (receptionistPrameters.SearchString is not null && receptionistPrameters.SearchString.Length > 0)
        {
            if (receptionistPrameters.Offices is null || receptionistPrameters.Offices.Count == 0)
            {
                query.Append($@"
            WHERE 
            CONCAT(Receptionists.FirstName, ' ', Receptionists.LastName, ' ', Receptionists.SecondName) LIKE '%{receptionistPrameters.SearchString}%' ");
            }
            else
            {
                query.Append($@"
            AND 
            CONCAT(Receptionists.FirstName, ' ', Receptionists.LastName, ' ', Receptionists.SecondName) LIKE '%{receptionistPrameters.SearchString}%' ");
            }
        }

        query.Append($@"
        ORDER BY Receptionists.Id
        OFFSET {(receptionistPrameters.PageNumber - 1) * receptionistPrameters.PageSize} ROWS 
        FETCH NEXT {receptionistPrameters.PageSize} ROWS ONLY; ");
        string finalQuery = query.ToString();
        using (var connection = _profilesDBContext.Connection)
        {
            var receptionists = await connection.QueryAsync<Receptionist>(finalQuery);
            return receptionists.ToList();
        }
    }

    public async Task<Receptionist> GetByIdAsync(Guid receptionistId)
    {
        var query = "Select * From Receptionists " +
            "Where Receptionists.Id = @ReceptionistId ";

        using (var connection = _profilesDBContext.Connection)
        {
            var receptionist = await connection.QueryFirstOrDefaultAsync<Receptionist>(query, new { receptionistId});
            return receptionist;
        }        
    }

    public async Task<bool> IsProfileExists(Guid userId)
    {
        var query = "Select * From Receptionists " +
            "Where Receptionists.UserId = @UserId ";
        using (var connection = _profilesDBContext.Connection)
        {
            var receptionist = await connection.QueryFirstOrDefaultAsync<Receptionist>(query, new { userId });

            return receptionist is not null;
        }
    }

    public async Task UpdateAsync(Guid receptionistId, Receptionist updatedReceptionist)
    {

        var query = "Update Receptionists " +
                    "Set WorkStatusId = @WorkStatusId, OfficeId = @OfficeId, FirstName = @FirstName, " +
                        "LastName = @LastName, SecondName = @SecondName, Address = @Address, WorkEmail = @WorkEmail, " +
                        "Phone = @Phone , BirthDate = @BirthDate, CareerStartDate = @CareerStartDate, " +
                        "Photo = @Photo, PhotoId = @PhotoId " +
                    "Where Id = @ReceptionistId ";

        var receptionistParameters = new DynamicParameters();
        receptionistParameters.Add("ReceptionistId", receptionistId, System.Data.DbType.Guid);
        receptionistParameters.Add("WorkStatusId", updatedReceptionist.WorkStatusId, System.Data.DbType.Guid);
        receptionistParameters.Add("OfficeId", updatedReceptionist.OfficeId, System.Data.DbType.String);
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
