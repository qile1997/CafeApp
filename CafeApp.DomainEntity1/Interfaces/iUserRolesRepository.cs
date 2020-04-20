using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iUserRolesRepository
    {
        IEnumerable<UserRoles> GetUserRoles();
        int FilterCashier();
        UserRoles userRoles(int? id);
        void AddUserRoles(UserRoles userRoles);
        void UpdateUserRoles(UserRoles userRoles);
        void DeleteUserRoles(UserRoles userRoles);
        void Save();
        void CreateTables(UserRoles userRoles);
        bool CheckEditDuplicateUser(UserRoles userRoles);
        bool CheckDuplicateUser(UserRoles userRoles);
        void CreateAdmin();
    }
}
