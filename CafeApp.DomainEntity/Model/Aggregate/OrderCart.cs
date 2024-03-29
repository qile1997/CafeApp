﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CafeApp.DomainEntity
{
    public class OrderCart
    {
        public int OrderCartId { get; set; }
        [ForeignKey("Foods")]
        public int FoodsId { get; set; }
        public virtual Food Foods { get; set; }
        [DisplayName("Quantity")]
        public int FoodQuantity { get; set; }
        [DisplayName("Total Amount (RM)")]
        public int TotalAmount { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}