using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iOrderCartRepository
    {
        IEnumerable<OrderCart> GetAllOrderCart();
        IEnumerable<Table> GetEmptyStatusTables();
        OrderCart GetOrderCartById(int? id);
        void AddOrderCart(OrderCart cart);
        void UpdateOrderCart(OrderCart cart);
        void RemoveOrderCart(OrderCart cart);
        void SaveChanges();
       
    }
}
