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
        void SaveChanges();
        void CancelOrder(int id);
        int FoodCount(int id);
        int FoodPriceSum(int id);
        void CartQuantity(int id, string _operator,int SessionId);
        string Cart(int id,int checkId);
        string ConfirmOrder(int id,int Seat);
        void ClearCart(int SessionId);
        Food FilterFood(int SessionId);
        Table CheckSeat(int SessionId);
        IEnumerable<OrderCart> OrderedFood(int id);
    }
}
