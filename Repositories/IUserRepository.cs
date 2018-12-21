using System;
namespace vacation_accrual_buddy.Repositories
{
    public interface IUserRepository
    {
        bool Exists(string userId);
    }
}
