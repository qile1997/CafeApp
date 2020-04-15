using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iOrderCartRepository
    {
        IEnumerable<OrderCart> GetOrderCarts();
        IEnumerable<Table> GetEmptyTables();
        OrderCart cart(int? id);
        void AddCart(OrderCart cart);
        void UpdateCart(OrderCart cart);
        void RemoveCart(OrderCart cart);
        void Save();
        int FoodCount(int id);
        int FoodPriceSum(int id);
        string CartQuantity(int id, string _operator);
        string Cart(int id,int checkId);
        string ConfirmOrder(int id,int Seat);
        string ClearCart();
        Foods FilterFood(int id);
        IEnumerable<OrderCart> OrderedFood(int id);
    }
}
