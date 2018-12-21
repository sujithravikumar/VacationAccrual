using System;
namespace vacation_accrual_buddy.Repositories
{
    public interface IUserPreferencesRepository
    {
        bool Exists(string userId);
    }
}
