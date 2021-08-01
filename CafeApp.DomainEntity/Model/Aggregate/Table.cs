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
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public TableStatus TableStatus { get; set; }
    }
    public enum TableStatus
    {
        Empty, Occupied
    }
}