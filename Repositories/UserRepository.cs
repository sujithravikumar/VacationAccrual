using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using vacation_accrual_buddy.Models;

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
                string query = @"SELECT EXISTS (SELECT 1 
                                               FROM   PUBLIC.user_data 
                                               WHERE  user_id = @userId)";
                return conn.ExecuteScalar<bool>(query, new { userId });
            }
        }

        public UserDataModel Get(string userId)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"SELECT *
                                FROM PUBLIC.user_data
                                WHERE user_id = @userId";
                var result = conn.QuerySingle<UserDataModel>(query, new { userId });
                return result;
            }
        }

        public void Insert(
            string userId,
            bool startDateEvenWW,
            decimal accrual,
            decimal maxBalance,
            int period)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"INSERT INTO PUBLIC.user_data 
                                            (user_id, 
                                             start_date_even_ww, 
                                             accrual, 
                                             max_balance, 
                                             period) 
                                VALUES      (@userId, 
                                             @startDateEvenWW, 
                                             @accrual, 
                                             @maxBalance, 
                                             @period)";
                int affectedRows = conn.Execute(
                                        query,
                                        new
                                        {
                                            userId,
                                            startDateEvenWW,
                                            accrual,
                                            maxBalance,
                                            period        
                                        });
            }
        }

        public void Update(
            string userId,
            bool startDateEvenWW,
            decimal accrual,
            decimal maxBalance,
            int period)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"UPDATE public.user_data
                                SET start_date_even_ww = @startDateEvenWW,
                                    accrual = @accrual,
                                    max_balance = @maxBalance,
                                    period = @period
                                WHERE user_id = @userId";
                int affectedRows = conn.Execute(
                                        query,
                                        new
                                        {
                                            userId,
                                            startDateEvenWW,
                                            accrual,
                                            maxBalance,
                                            period
                                        });
            }
        }
    }
}
