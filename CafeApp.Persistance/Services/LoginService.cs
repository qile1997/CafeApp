using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.Persistance.Repositories;
using System.Linq;

namespace CafeApp.Persistance.Services
{
    public class LoginService : iLoginService
    {
        private CafeWebApp _context;
        public iUserRolesRepository UserRolesRepository { get; }

        public LoginService(CafeWebApp db)
        {
            _context = db;
            UserRolesRepository = new UserRolesRepository(_context);
        }

        public UserRoles LoginUserRole(UserRoles userRoles)
        {
            var LoginUser = _context.UserRoles.Where(d => d.Username == userRoles.Username && d.Password == userRoles.Password).SingleOrDefault();
            return LoginUser;
        }
    }
}
