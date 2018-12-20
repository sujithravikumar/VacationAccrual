using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace vacation_accrual_buddy.Repositories
{
    public class UserPreferencesRepository : IUserPreferencesRepository
    {
        private readonly IConfiguration _configuration;

        public UserPreferencesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_configuration["CUSTOMCONNSTR_Database"]);
            }
        }

        public bool UserPreferencesRecordExist()
        {
            using (IDbConnection conn = Connection)
            {
                string query = "SELECT 555";
                var result = conn.Query(query);
                return result != null;
            }
        }
    }
}
