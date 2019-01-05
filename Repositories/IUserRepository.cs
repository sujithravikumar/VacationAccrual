using vacation_accrual_buddy.Models;
namespace vacation_accrual_buddy.Repositories
{
    public interface IUserRepository
    {
        bool Exists(string userId);

        UserDataModel Get(string userId);

        void Insert(
            string userId,
            bool cycleStartEvenWW,
            decimal accrual,
            decimal maxBalance,
            int period,
            decimal takeDaysOff);

        void Update(
            string userId,
            bool cycleStartEvenWW,
            decimal accrual,
            decimal maxBalance,
            int period,
            decimal takeDaysOff);
    }
}
