using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                return new NpgsqlConnection(_configuration["Database_ConnStr"]);
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
                               FROM   PUBLIC.vacation_data
                               WHERE  user_id = @userId
                                      AND start_date = To_date(@startDate, 'YYYY-MM-DD')
                                      AND end_date = To_date(@endDate, 'YYYY-MM-DD'))";
                return conn.ExecuteScalar<bool>(
                                    query,
                                    new {
                                        userId,
                                        startDate = startDate.ToString("yyyy-MM-dd"),
                                        endDate = endDate.ToString("yyyy-MM-dd")
                                    });
            }
        }

        public List<PayPeriod> Get(
            string userId,
            DateTime startDate,
            int period)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"SELECT   *
                                FROM     PUBLIC.vacation_data_view
                                WHERE    user_id = @userId
                                AND      start_date >= To_date(@startDate, 'YYYY-MM-DD')
                                ORDER BY start_date limit @period";
                return conn.Query<PayPeriod>(
                                    query,
                                    new {
                                        userId,
                                        startDate = startDate.ToString("yyyy-MM-dd"),
                                        period
                                    }).ToList();
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
                                             To_date(@startDate, 'YYYY-MM-DD'),
                                             To_date(@endDate, 'YYYY-MM-DD'),
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
                                       AND start_date = To_date(@startDate, 'YYYY-MM-DD')
                                       AND end_date = To_date(@endDate, 'YYYY-MM-DD')";
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

        public void Delete(string userId)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"DELETE FROM PUBLIC.vacation_data
                                WHERE  user_id = @userId";
                int affectedRows = conn.Execute(query, new { userId });
            }
        }
    }
}
