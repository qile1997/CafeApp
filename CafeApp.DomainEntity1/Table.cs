using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CafeApp.DomainEntity
{
    public class Table
    {
        public int TableId { get; set; }
        public int TableNo { get; set; }
        [ForeignKey("UserRoles")]
        public int? UserRolesId { get; set; }
        public virtual UserRoles UserRoles { get; set; }
        public TableStatus TableStatus { get; set; }
    }
    public enum TableStatus
    {
        Empty, Occupied
    }
}