using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.DomainEntity.ViewModel;
using CafeApp.Persistance.Repositories;
using System.Linq;

namespace CafeApp.Persistance.Services
{
    public class UserService : iUserService
    {

        //private CafeWebApp _context;
        //public iUserRepository UserRepository { get; }
        //public iTableRepository TableRepository { get; }

        //public UserService(CafeWebApp context)
        //{
        //    _context = context;
        //    UserRepository = new UserRepository(_context);
        //    TableRepository = new TableRepository(_context);
        //}
        private CafeWebApp _context = new CafeWebApp();
        private UserRepository UserRepository = new UserRepository();
        private TableRepository TableRepository = new TableRepository();
        private UserService _userService = new UserService();
        private FoodRepository FoodRepository = new FoodRepository();
        private OrderCartRepository OrderCartRepository = new OrderCartRepository();
        public void CreateTables(User userRoles)
        {
            if (userRoles.Roles == Roles.Cashier && TableRepository.GetTables().Count() < 10)
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
            }
            SaveChanges();
        }

        public bool CheckEditDuplicateUser(User userRoles)
        {
            var filterUser = _context.Users.Where(d => d.UserId == userRoles.UserId).SingleOrDefault();
            var editValidation1 = _context.Users.Where(d => d.Username != filterUser.Username && d.Roles == filterUser.Roles).ToList();
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

        public bool CheckDuplicateUser(User userRoles)
        {
            var filterUser = _context.Users.Where(d => d.Username == userRoles.Username && d.Roles == userRoles.Roles).SingleOrDefault();

            if (filterUser != null)
            {
                return true;
            }
            return false;
        }
        //When you initialize data without any users
        public void CreateAdmin()
        {
            if (UserRepository.GetAllUsers().Count() < 1)
            {
                User user = new User();
                user.Username = "admin";
                user.Password = "admin";
                user.Roles = Roles.Admin;
                _context.Users.Add(user);
                SaveChanges();
            }
        }

        public int GetAllCashier()
        {
            return _context.Users.Where(d => d.Roles == Roles.Cashier).ToList().Count();
        }
        public User CheckUserCredentials(LoginCredentialsViewModel userCredentials, Roles roles)
        {
            CafeWebApp _context = new CafeWebApp();
            var user = _context.Users.Where(d => d.Username == userCredentials.Username && d.Password == userCredentials.Password && d.Roles == roles).SingleOrDefault();

            return user != null ? user : null;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
