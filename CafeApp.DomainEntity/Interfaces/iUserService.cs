

using CafeApp.DomainEntity.ViewModel;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iUserService
    {
        int GetAllCashier();
        void CreateTables(User user);
        bool CheckEditDuplicateUser(User user);
        bool CheckDuplicateUser(User user);
        void CreateAdmin();
        void SaveChanges();
        User CheckUserCredentials(LoginCredentialsViewModel user, Roles roles);
    }
}
