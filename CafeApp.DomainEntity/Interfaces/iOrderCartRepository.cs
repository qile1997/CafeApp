using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iOrderCartRepository
    {
        IEnumerable<OrderCart> GetOrderCarts();
        OrderCart cart(int? id);
        void AddCart(OrderCart cart);
        void UpdateCart(OrderCart cart);
        void RemoveCart(OrderCart cart);
        void Save();
        int FoodCount(int id);
        int FoodPriceSum(int id);
        IEnumerable<OrderCart> OrderedFood(int id);
    }
}
