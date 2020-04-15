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
    public class OrderCartRepository : iOrderCartRepository
    {
        CafeWebApp db = new CafeWebApp();
        public void AddCart(OrderCart cart)
        {
            db.OrderCart.Add(cart);
            Save();
        }

        public OrderCart cart(int? id)
        {
            OrderCart cart = db.OrderCart.Find(id);
            return cart;
        }

        public int FoodCount(int checkId)
        {
            var filterCount = db.OrderCart.Where(d => d.UserRolesId == checkId).ToList();
            int Count = 0;

            foreach (var item in filterCount)
            {
                Count += item.FoodQuantity;
            }
            return Count;
        }

        public int FoodPriceSum(int checkId)
        {
            var filterCount = db.OrderCart.Where(d => d.UserRolesId == checkId).ToList();

            int TotalAmount = 0;
            foreach (var item in filterCount)
            {
                TotalAmount += item.TotalAmount;
            }
            return TotalAmount;
        }

        public IEnumerable<OrderCart> GetOrderCarts()
        {
            return db.OrderCart.ToList();
        }

        public IEnumerable<OrderCart> OrderedFood(int checkId)
        {
            var filterCount = db.OrderCart.Where(d => d.UserRolesId == checkId).ToList();
            return filterCount;
        }

        public void RemoveCart(OrderCart cart)
        {
            db.OrderCart.Remove(cart);
            Save();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateCart(OrderCart cart)
        {
            db.Entry(cart).State = EntityState.Modified;
        }
    }
}
