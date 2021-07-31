using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.Persistance.Repositories;
using System.Linq;

namespace CafeApp.Persistance.Services
{
    public class UserRolesService : iUserRolesService
    {
        private CafeWebApp _context;
        public iUserRolesRepository UserRolesRepository { get; }
        public iTableRepository TableRepository { get; }

        public UserRolesService(CafeWebApp context)
        {
            _context = context;
            UserRolesRepository = new UserRolesRepository(_context);
            TableRepository = new TableRepository(_context);
        }
        public void CreateTables(UserRoles userRoles)
        {
            if (userRoles.Roles == Roles.Cashier && TableRepository.GetTables().Count() < 10)
            {
                for (int i = 1; i <= 10; i++)
                {
                    Table table = new Table();
                    table.TableId = i;
                    table.TableNo = i;
                    table.TableStatus = TableStatus.Empty;
                    table.UserRolesId = null;
                    _context.Table.Add(table);
                }
            }
            SaveChanges();
        }
        public bool CheckEditDuplicateUser(UserRoles userRoles)
        {
            var filterUser = _context.UserRoles.Where(d => d.UserRolesId == userRoles.UserRolesId).SingleOrDefault();
            var editValidation1 = _context.UserRoles.Where(d => d.Username != filterUser.Username && d.Roles == filterUser.Roles).ToList();
            var editValidation2 = editValidation1.Where(d => d.Username == userRoles.Username).SingleOrDefault();

            if (editValidation2 != null)
            {
                return true;
            }

            filterUser.Username = userRoles.Username;
            filterUser.Password = userRoles.Password;
            filterUser.Roles = userRoles.Roles;
            SaveChanges();
            return false;
        }
        public bool CheckDuplicateUser(UserRoles userRoles)
        {
            var filterUser = _context.UserRoles.Where(d => d.Username == userRoles.Username && d.Roles == userRoles.Roles).SingleOrDefault();

            if (filterUser != null)
            {
                return true;
            }
            return false;
        }
        //When you initialize data without any users
        public void CreateAdmin()
        {
            var createAdminfilter = UserRolesRepository.GetUserRoles().Count();

            if (createAdminfilter < 1)
            {
                UserRoles user = new UserRoles();
                user.Username = "admin";
                user.Password = "admin";
                user.Roles = Roles.Admin;
                _context.UserRoles.Add(user);
                SaveChanges();
            }
        }

        public int FilterCashier()
        {
            var Count = _context.UserRoles.Where(d => d.Roles == Roles.Cashier).ToList();
            return Count.Count();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
