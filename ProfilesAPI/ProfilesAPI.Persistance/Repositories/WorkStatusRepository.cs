using Dapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class WorkStatusRepository : IWorkStatusRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public WorkStatusRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task<Guid> CreateAsync(WorkStatus workStatus)
    {
        var query = "Insert into WorkStatuses (Id, Title, Description)" +
            "OUTPUT Inserted.ID " +
            "Values (@Id, @Title, @Description); ";

        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.NewGuid(), System.Data.DbType.Guid);
        parameters.Add("Title", workStatus.Title, System.Data.DbType.String);
        parameters.Add("Description", workStatus.Description, System.Data.DbType.String);

        using (var connection = _profilesDBContext.Connection)
        {
            var workStatusId = await connection.ExecuteScalarAsync<Guid>(query, parameters);

            return workStatusId;
        }
    }

    public async Task DeleteByIdAsync(Guid workStatusId)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Delete From WorkStatuses " +
                "Where WorkStatuses.Id = @WorkStatusId ";
            await connection.ExecuteAsync(query, new { workStatusId });
        }
    }

    public async Task<ICollection<WorkStatus>> GetAllAsync()
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Select * From WorkStatuses ";
            var workStatuses = await connection.QueryAsync<WorkStatus>(query);

            return workStatuses.ToList();
        }
    }

    public async Task<WorkStatus> GetByIdAsync(Guid workStatusId)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Select * From WorkStatuses " +
                "Where WorkStatuses.Id = @WorkStatusId ";
            var workStatus = await connection.QueryFirstOrDefaultAsync<WorkStatus>(query, new { workStatusId });

            return workStatus;
        }    
    }

    public async Task UpdateAsync(Guid workStatusId, WorkStatus updatedWorkStatus)
    {
        using (var connection = _profilesDBContext.Connection)
        {
            var query = "Update WorkStatuses " +
                "Set Title = @Title, Description = @Description " +
                "Where WorkStatuses.Id = @WorkStatusId ";

            var parameters = new DynamicParameters();
            parameters.Add("WorkStatusId", workStatusId, System.Data.DbType.Guid);
            parameters.Add("Title", updatedWorkStatus.Title, System.Data.DbType.String);
            parameters.Add("Description", updatedWorkStatus.Description, System.Data.DbType.String);
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
