using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;

namespace CafeApp.Persistance.Repositories
{
    public class LoginRepository : iLoginRepository
    {
        private CafeWebApp _context;
        public LoginRepository(CafeWebApp context)
        {
            _context = context;
        }
        public User Login(User userRoles)
        {
            var LoginUser = _context.Users.Where(d => d.Username == userRoles.Username && d.Password == userRoles.Password).Single();
            return LoginUser;
        }
    }
}
