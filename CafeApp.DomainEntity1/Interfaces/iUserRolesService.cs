using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iUserRolesService
    {
        int FilterCashier();
        void CreateTables(UserRoles userRoles);
        bool CheckEditDuplicateUser(UserRoles userRoles);
        bool CheckDuplicateUser(UserRoles userRoles);
        void CreateAdmin();
        void SaveChanges();
    }
}
