using System;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using vacation_accrual_buddy.Models;

namespace vacation_accrual_buddy.Repositories
{
    public class VacationRepository : IVacationRepository
    {
        private readonly IConfiguration _configuration;

        public VacationRepository(IConfiguration configuration)
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

        public bool Exists(
            string userId,
            DateTime startDate,
            DateTime endDate)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"SELECT EXISTS (SELECT 1
                               FROM   PUBLIC.user_data
                               WHERE  user_id = @userId
                                      AND start_date = TO_DATE(@startDate, 'YYYY-MM-DD')
                                      AND end_date = TO_DATE(@endDate, 'YYYY-MM-DD'))";
                return conn.ExecuteScalar<bool>(
                                    query,
                                    new {
                                        userId,
                                        startDate = startDate.ToString("yyyy-MM-dd"),
                                        endDate = endDate.ToString("yyyy-MM-dd")
                                    });
            }
        }

        public VacationDataModel Get(
            string userId,
            DateTime startDate,
            int period)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"SELECT *
                                FROM PUBLIC.vacation_data
                                WHERE user_id = @userId"; // FIXME
                return conn.QuerySingle<VacationDataModel>(query, new { userId });
            }
        }

        public void Insert(
            string userId,
            DateTime startDate,
            DateTime endDate,
            decimal accrual,
            decimal take,
            decimal balance,
            decimal forfeit)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"INSERT INTO PUBLIC.vacation_data
                                            (user_id,
                                             start_date,
                                             end_date,
                                             accrual,
                                             take,
                                             balance,
                                             forfeit)
                                VALUES      (@userId,
                                             TO_DATE(@startDate, 'YYYY-MM-DD'),
                                             TO_DATE(@endDate, 'YYYY-MM-DD'),
                                             @accrual,
                                             @take,
                                             @balance,
                                             @forfeit)";
                int affectedRows = conn.Execute(
                                        query,
                                        new
                                        {
                                            userId,
                                            startDate = startDate.ToString("yyyy-MM-dd"),
                                            endDate = endDate.ToString("yyyy-MM-dd"),
                                            accrual,
                                            take,
                                            balance,
                                            forfeit
                                        });
            }
        }

        public void Update(
            string userId,
            DateTime startDate,
            DateTime endDate,
            decimal take,
            decimal balance,
            decimal forfeit)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"UPDATE PUBLIC.vacation_data
                                SET    take = @take,
                                       balance = @balance,
                                       forfeit = @forfeit
                                WHERE  user_id = @userId
                                       AND start_date = TO_DATE(@startDate, 'YYYY-MM-DD')
                                       AND end_date = TO_DATE(@endDate, 'YYYY-MM-DD')";
                int affectedRows = conn.Execute(
                                        query,
                                        new
                                        {
                                            userId,
                                            startDate = startDate.ToString("yyyy-MM-dd"),
                                            endDate = endDate.ToString("yyyy-MM-dd"),
                                            take,
                                            balance,
                                            forfeit
                                        });
            }
        }
    }
}
