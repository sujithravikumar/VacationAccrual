using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace vacation_accrual_buddy.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
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

        public bool Exists(string userId)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"SELECT exists
                                  (SELECT 1
                                   FROM public.user_data
                                   WHERE user_id = @userId)";
                return conn.ExecuteScalar<bool>(query, new { userId });
            }
        }
    }
}
