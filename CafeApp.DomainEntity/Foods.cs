using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeApp.DomainEntity
{
    public class Foods
    {
        public int FoodsId { get; set; }
        public ICollection<OrderCart> OrderCart { get; set; }
        [DisplayName("Food Category")]
        public Category FoodCategory { get; set; }
        [DisplayName("Food Name")]
        public string FoodName { get; set; }
        //[NotMapped]
        //public HttpPostedFileBase Photo { get; set; }
        public string PhotoFile { get; set; }
        [DisplayName("Price (RM)")]
        public int Price { get; set; }
        public string Remarks { get; set; }
    }
    public enum Category
    {
        Rice, Noodles, Egg
    }

}