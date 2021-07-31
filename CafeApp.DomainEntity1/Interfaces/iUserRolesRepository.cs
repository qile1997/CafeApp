using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iUserRolesRepository
    {
        IEnumerable<UserRoles> GetUserRoles();
        UserRoles userRoles(int? id);
        void AddUserRoles(UserRoles userRoles);
        void UpdateUserRoles(UserRoles userRoles);
        void DeleteUserRoles(UserRoles userRoles);
        void SaveChanges();
 
    }
}
