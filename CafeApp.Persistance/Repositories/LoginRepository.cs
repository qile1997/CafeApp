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
        private CafeWebApp db = new CafeWebApp();
        public UserRoles Login(UserRoles userRoles)
        {
            var LoginUser = db.UserRoles.Where(d => d.Username == userRoles.Username && d.Password == userRoles.Password).SingleOrDefault();
            return LoginUser;
        }
    }
}
