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
        private CafeWebApp db = new CafeWebApp();
        public void AddUserRoles(UserRoles userRoles)
        {
            db.UserRoles.Add(userRoles);
            Save();
        }

        public void CreateTables(UserRoles userRoles)
        {
            var filterTable = db.Table.Where(d => d.TableId < 1).ToList();

            if (userRoles.Roles == Roles.Cashier && filterTable.Count < 1)
            {
                for (int i = 1; i <= 10; i++)
                {
                    Table table = new Table();
                    table.TableId = i;
                    table.TableNo = "T" + i;
                    table.TableStatus = TableStatus.Empty;
                    table.UserRolesId = null;
                    db.Table.Add(table);
                }
            }
            Save();
        }

        public void DeleteUserRoles(UserRoles userRoles)
        {
            db.UserRoles.Remove(userRoles);
            Save();
        }

        public IEnumerable<UserRoles> GetUserRoles()
        {
            return db.UserRoles.ToList();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public bool CheckEditDuplicateUser(UserRoles userRoles)
        {
            var filterUser = db.UserRoles.Where(d => d.UserRolesId == userRoles.UserRolesId).SingleOrDefault();
            var editValidation1 = db.UserRoles.Where(d => d.Username != filterUser.Username && d.Roles == filterUser.Roles).ToList();
            var editValidation2 = editValidation1.Where(d => d.Username == userRoles.Username).SingleOrDefault();

            if (editValidation2 != null)
            {
                return true;
            }

            filterUser.Username = userRoles.Username;
            filterUser.Password = userRoles.Password;
            filterUser.Roles = userRoles.Roles;
            Save();
            return false;
        }

        public void UpdateUserRoles(UserRoles userRoles)
        {
            db.Entry(userRoles).State = EntityState.Modified;
        }

        public UserRoles userRoles(int? id)
        {
            UserRoles userRoles = db.UserRoles.Find(id);
            return userRoles;
        }

        public bool CheckDuplicateUser(UserRoles userRoles)
        {
            var filterUser = db.UserRoles.Where(d => d.Username == userRoles.Username && d.Roles == userRoles.Roles).SingleOrDefault();

            if (filterUser != null)
            {
                return true;
            }
            return false;
        }

        public UserRoles Login(UserRoles userRoles)
        {
            var LoginUser = db.UserRoles.Where(d => d.Username == userRoles.Username && d.Password == userRoles.Password && d.Roles == Roles.Admin).SingleOrDefault();

            return LoginUser;

        }

        public void CreateAdmin()
        {
            var createAdminfilter = db.UserRoles.Where(d => d.UserRolesId > 1).ToList();

            if (createAdminfilter.Count < 1)
            {
                UserRoles user = new UserRoles();
                user.Username = "admin";
                user.Password = "admin";
                user.Roles = Roles.Admin;
                db.UserRoles.Add(user);
                Save();
            }

        }
    }
}
