using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CafeApp.DomainEntity
{
    public class UserRoles
    {
        public int? UserRolesId { get; set; }
        public ICollection<OrderCart> OrderCart { get; set; }
        public ICollection<Table> Table { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Roles Roles { get; set; }
    }
    public enum Roles
    {
        Customer, Admin, Cashier
    }
}