using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iUserRepository
    {
        void AddUser(User user);
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllCashier();
        User GetUserById(int? id);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void SaveChanges();
 
    }
}
