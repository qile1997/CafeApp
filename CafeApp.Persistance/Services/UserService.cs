using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.DomainEntity.ViewModel;
using CafeApp.Persistance.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CafeApp.Persistance.Services
{
    public class UserService : iUserService
    {

        private CafeWebApp _context = new CafeWebApp();
        private UserRepository _userRepository = new UserRepository();
        private TableRepository _tableRepository = new TableRepository();

        public void CreateTables(User userRoles)
        {
            if (userRoles.Roles == Roles.Cashier && _tableRepository.GetAllTables().Count() < 10)
            {
                for (int i = 1; i <= 10; i++)
                {
                    Table table = new Table();
                    table.TableId = i;
                    table.TableNo = i;
                    table.TableStatus = TableStatus.Empty;
                    table.UserId = null;
                    _context.Table.Add(table);
                }
                SaveChanges();
            }  
        }

        public void CreateAdmin()
        {
            if (_userRepository.GetAllUsers().Count() < 1)
            {
                User user = new User();
                user.Username = "admin";
                user.Password = "admin";
                user.Roles = Roles.Admin;
                _context.Users.Add(user);
                SaveChanges();
            }
        }
        public bool CheckDuplicateUser_EditMode(User user)
        {
            var initUser = _context.Users.Where(d => d.UserId == user.UserId).SingleOrDefault();
            var notInitUser = _context.Users.Where(d => d.Username != initUser.Username && d.Roles == initUser.Roles).ToList();
            var duplicateUserExist = notInitUser.Where(d => d.Username == user.Username).SingleOrDefault();

            if (duplicateUserExist != null)
            {
                return true;
            }

            initUser.Username = user.Username;
            initUser.Password = user.Password;
            //initUser.Roles = user.Roles;
            SaveChanges();
            return false;
        }

        public bool CheckDuplicateUser(User user)
        {
            return _context.Users.Where(d => d.Username == user.Username && d.Roles == user.Roles).SingleOrDefault() != null ? true : false;
        }
        public User CheckUserCredentials(LoginCredentialsViewModel userCredentials, Roles roles)
        {
            var user = _context.Users.Where(d => d.Username == userCredentials.Username && d.Password == userCredentials.Password && d.Roles == roles).SingleOrDefault();

            return user != null ? user : null;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
