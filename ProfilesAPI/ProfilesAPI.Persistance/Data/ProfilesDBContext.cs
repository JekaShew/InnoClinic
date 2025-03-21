using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ProfilesAPI.Persistance.Data
{
    public class ProfilesDBContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _masterConnectionString;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;

        // ProfilesDBDockerFromLocal
        // MasterDBDockerFromLocal
        public ProfilesDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ProfilesDBDockerFromLocal");
            _masterConnectionString = _configuration.GetConnectionString("MasterDBDockerFromLocal");
        }

        public IDbConnection? Connection
        {
            get
            {
                if(_dbConnection is null || !_dbConnection.State.Equals(ConnectionState.Open) )
                {
                    _dbConnection = new SqlConnection(_connectionString);
                }
                return _dbConnection;
            }
        }

        public IDbConnection? MasterConnection
        {
            get
            {
                if (_dbConnection is null || !_dbConnection.State.Equals(ConnectionState.Open))
                {
                    _dbConnection = new SqlConnection(_masterConnectionString);
                }
                return _dbConnection;
            }
        }

        public IDbTransaction? Transaction
        {
            get
            {
                return _dbTransaction;
            }
            set
            {
                _dbTransaction = value;
            }
        }
    }
}
