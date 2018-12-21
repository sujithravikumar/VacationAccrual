using System;
namespace vacation_accrual_buddy.Repositories
{
    public interface IUserRepository
    {
        bool Exists(string userId);
        void Insert(
            string userId,
            bool cycleStartEvenWW,
            decimal accrual,
            decimal maxBalance,
            int period);
    }
}
