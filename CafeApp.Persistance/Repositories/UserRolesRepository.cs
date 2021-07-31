using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApp.Persistance.Repositories
{
    public class UserRolesRepository : iUserRolesRepository
    {
        private CafeWebApp _context;

        public UserRolesRepository(CafeWebApp context)
        {
            _context = context;
        }
        public void AddUserRoles(UserRoles userRoles)
        {
            _context.UserRoles.Add(userRoles);
            SaveChanges();
        }

        public void DeleteUserRoles(UserRoles userRoles)
        {
            _context.UserRoles.Remove(userRoles);
            SaveChanges();
        }

        public IEnumerable<UserRoles> GetUserRoles()
        {
            return _context.UserRoles.ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateUserRoles(UserRoles userRoles)
        {
            _context.Entry(userRoles).State = EntityState.Modified;
        }

        public UserRoles userRoles(int? id)
        {
            UserRoles userRoles = _context.UserRoles.Find(id);
            return userRoles;
        }   
    }
}
