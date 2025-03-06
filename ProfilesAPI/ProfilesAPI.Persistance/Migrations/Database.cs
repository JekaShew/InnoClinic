using Dapper;
using ProfilesAPI.Persistance.Data;
using System.Xml.Linq;

namespace ProfilesAPI.Persistance.Migrations;

public class Database
{
    private readonly ProfilesDBContext _profilesDBContext;

    public Database(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public void CreateDatabase(string dbTitle)
    {
        var query = "SELECT * FROM sys.databases WHERE name = @name";
        var parameters = new DynamicParameters();
        parameters.Add("name", dbTitle);
        using (var connection = _profilesDBContext.MasterConnection)
        {
            var records = connection.Query(query, parameters);
            if (!records.Any())
                connection.Execute($"CREATE DATABASE {dbTitle}");
            connection.Close();
        }       
    }

    public void DropDatabase(string dbTitle)
    {
        var query = "SELECT * FROM sys.databases WHERE name = @name";
        var parameters = new DynamicParameters();
        parameters.Add("name", dbTitle);
        using (var connection = _profilesDBContext.MasterConnection)
        {
            var records = connection.Query(query, parameters);
            if (!records.Any())
                connection.Execute($"DROP DATABASE {dbTitle}");
            connection.Close();
        }
    }
}
