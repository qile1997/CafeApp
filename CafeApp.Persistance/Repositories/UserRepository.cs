using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.Persistance.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApp.Persistance.Repositories
{
    public class UserRepository : iUserRepository
    {
        private CafeWebApp _context = new CafeWebApp();

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            SaveChanges();
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        public IEnumerable<User> GetAllCashier()
        {
            return _context.Users.Where(d => d.Roles == Roles.Cashier).ToList();
        }
        public void UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            SaveChanges();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public User GetUserById(int? id)
        {
            User user = _context.Users.Find(id);
            return user;
        }   
    }
}
