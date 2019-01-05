using System;
using System.Collections.Generic;
using vacation_accrual_buddy.Models;
namespace vacation_accrual_buddy.Repositories
{
    public interface IVacationRepository
    {
        bool Exists(
            string userId,
            DateTime startDate,
            DateTime endDate);

        List<PayPeriod> Get(
            string userId,
            DateTime startDate,
            int period);

        void Insert(
            string userId,
            DateTime startDate,
            DateTime endDate,
            decimal accrual,
            decimal take,
            decimal balance,
            decimal forfeit);

        void Update(
            string userId,
            DateTime startDate,
            DateTime endDate,
            decimal take,
            decimal balance,
            decimal forfeit);

        void Delete(string userId);
    }
}
