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
                return new NpgsqlConnection(_configuration["Database_ConnStr"]);
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
                return conn.QuerySingle<UserDataModel>(query, new { userId });
            }
        }

        public void Insert(
            string userId,
            bool startDateEvenWW,
            decimal accrual,
            decimal maxBalance,
            int period,
            decimal takeDaysOff,
            bool emailAlertEnabled,
            int emailAlertDaysBefore)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"INSERT INTO PUBLIC.user_data 
                                            (user_id, 
                                             start_date_even_ww, 
                                             accrual, 
                                             max_balance, 
                                             period,
                                             take_days_off,
                                             email_alert_enabled,
                                             email_alert_days_before) 
                                VALUES      (@userId, 
                                             @startDateEvenWW, 
                                             @accrual, 
                                             @maxBalance, 
                                             @period,
                                             @takeDaysOff,
                                             @emailAlertEnabled,
                                             @emailAlertDaysBefore)";
                int affectedRows = conn.Execute(
                                        query,
                                        new
                                        {
                                            userId,
                                            startDateEvenWW,
                                            accrual,
                                            maxBalance,
                                            period,
                                            takeDaysOff,
                                            emailAlertEnabled,
                                            emailAlertDaysBefore
                                        });
            }
        }

        public void Update(
            string userId,
            bool startDateEvenWW,
            decimal accrual,
            decimal maxBalance,
            int period,
            decimal takeDaysOff,
            bool emailAlertEnabled,
            int emailAlertDaysBefore)
        {
            using (IDbConnection conn = Connection)
            {
                string query = @"UPDATE public.user_data
                                SET start_date_even_ww = @startDateEvenWW,
                                    accrual = @accrual,
                                    max_balance = @maxBalance,
                                    period = @period,
                                    take_days_off = @takeDaysOff,
                                    email_alert_enabled = @emailAlertEnabled,
                                    email_alert_days_before = @emailAlertDaysBefore
                                WHERE user_id = @userId";
                int affectedRows = conn.Execute(
                                        query,
                                        new
                                        {
                                            userId,
                                            startDateEvenWW,
                                            accrual,
                                            maxBalance,
                                            period,
                                            takeDaysOff,
                                            emailAlertEnabled,
                                            emailAlertDaysBefore
                                        });
            }
        }
    }
}
