using Dapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class SpecializationRepository : ISpecializationRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public SpecializationRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task CreateAsync(Specialization specialization)
    {
        var query = "Insert into Specializations (Id, Title, Description, IsDelete) " +
            "Values (@Id, @Title, @Description, @IsDelete); ";

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.NewGuid(), System.Data.DbType.Guid);
        parameters.Add("Title", specialization.Title, System.Data.DbType.String);
        parameters.Add("Description", specialization.Description, System.Data.DbType.String);
        parameters.Add("IsDelete", specialization.IsDelete, System.Data.DbType.Boolean);

        using (var connection = _profilesDBContext.Connection)
        {
             await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task DeleteAsync(Specialization specialization)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Delete From Specializations " +
                "Where Specializations.Id = @SpecializationId ";
            await connection.ExecuteAsync(query, new { specialization .Id});
        }
    }

    public async Task<ICollection<Specialization>> GetAllAsync()
    {
        using(var connection = _profilesDBContext.Connection)
        {
            var query = "Select * From Specializations ";
            var specializations = await connection.QueryAsync<Specialization>(query);

            return specializations.ToList();
        }
    }

    public async Task<Specialization> GetByIdAsync(Guid specializationId)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Select * From Specializations " +
                "Where Specializations.Id = @SpecializationId ";
            var specialization = await connection.QueryFirstOrDefaultAsync<Specialization>(query, new { specializationId});
            
            return specialization;
        }
    }

    public async Task UpdateAsync(Guid specializationId, Specialization updatedSpecialization)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Update Specializations " +
                "Set Title = @Title, Description = @Description, IsDelete = @IsDelete " +
                "Where Specializations.Id = @SpecializationId ";
            var parameters = new DynamicParameters();
            parameters.Add("SpecializationId", specializationId, System.Data.DbType.Guid);
            parameters.Add("Title", updatedSpecialization.Title, System.Data.DbType.String);
            parameters.Add("Description", updatedSpecialization.Description, System.Data.DbType.String);
            parameters.Add("IsDelete", updatedSpecialization.IsDelete, System.Data.DbType.Boolean);
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
