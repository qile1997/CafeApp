using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CafeApp.Persistance.Repositories
{
    public class OrderCartRepository : iOrderCartRepository
    {
        private CafeWebApp _context = new CafeWebApp();
        public void AddOrderCart(OrderCart cart)
        {
            _context.OrderCart.Add(cart);
            SaveChanges();
        }

        public OrderCart GetOrderCartById(int? id)
        {
            return _context.OrderCart.Find(id);
        }

        public void RemoveOrderCart(OrderCart cart)
        {
            _context.OrderCart.Remove(cart);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateOrderCart(OrderCart cart)
        {
            _context.Entry(cart).State = EntityState.Modified;
        }
     
        public IEnumerable<Table> GetEmptyStatusTables()
        {
            return _context.Table.Where(d => d.TableStatus == TableStatus.Empty).ToList();
        }

        public IEnumerable<OrderCart> GetAllOrderCart()
        {
            return _context.OrderCart.ToList();
        }
    }
}
