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
        private CafeWebApp _context;
        public OrderCartRepository(CafeWebApp context)
        {
            _context = context;
        }
        public void AddCart(OrderCart cart)
        {
            _context.OrderCart.Add(cart);
            SaveChanges();
        }

        public OrderCart cart(int? id)
        {
            OrderCart cart = _context.OrderCart.Find(id);
            return cart;
        }

        public string Cart(int Id, int SessionId)
        {
            //var checkId = Convert.ToInt32(Session["CustomerId"]);
            OrderCart cart = new OrderCart();

            //Filter cart/food ordered that belongs to the customerID
            var filterCart = _context.OrderCart.Where(d => d.FoodsId == Id && d.UserId == SessionId).SingleOrDefault();
            //Filter food that belongs to the customerID
            var filterFood = FilterFood(Id);

            if (filterCart != null)
            {
                filterCart.FoodQuantity++;
                filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                SaveChanges();
                return filterFood.FoodName + " added into quantity .";
            }
            else
            {
                cart.FoodsId = Id;
                cart.TotalAmount = filterFood.Price;
                cart.FoodQuantity = 1;
                cart.UserId = SessionId;

                AddCart(cart);
                return filterFood.FoodName + " added into order .";
            }
        }

        public void CartQuantity(int FoodsId, string _operator, int SessionId)
        {
            var filterCart = _context.OrderCart.Where(d => d.FoodsId == FoodsId && d.UserId == SessionId).SingleOrDefault();
            var filterFood = FilterFood(FoodsId);
            if (_operator == "+")
            {
                filterCart.FoodQuantity++;
                filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                SaveChanges();
                return;
            }
            else if (_operator == "x")
            {
                RemoveCart(filterCart);
                return;
            }
            else
            {
                if (filterCart.FoodQuantity > 0)
                {
                    filterCart.FoodQuantity--;
                    if (filterCart.FoodQuantity == 0)
                    {
                        RemoveCart(filterCart);
                        return;
                    }
                    filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                    SaveChanges();
                    return;
                }
            }
        }
        public Food FilterFood(int Id)
        {
            return _context.Foods.Where(d => d.FoodId == Id).SingleOrDefault();
        }

        public void ClearCart(int SessionId)
        {
            var orderedFood = OrderedFood(SessionId);
            foreach (var item in orderedFood)
            {
                _context.OrderCart.Remove(item);
            }
            SaveChanges();
        }
        public void CancelOrder(int SessionId)
        {
            ClearCart(SessionId);
            var checkSeat = CheckSeat(SessionId);
            checkSeat.TableStatus = TableStatus.Empty;
            checkSeat.UserId = null;
            SaveChanges();
        }

        public string ConfirmOrder(int SessionId, int Seat)
        {
            var user = _context.Users.Where(d => d.UserId == SessionId).SingleOrDefault();
            var checkCurrentSeat = CheckSeat(SessionId);
            var replaceSeat = _context.Table.Where(d => d.TableId == Seat).SingleOrDefault();
            var filterfood = OrderedFood(SessionId);

            if (checkCurrentSeat != null)
            {
                if (checkCurrentSeat.TableStatus == TableStatus.Occupied)
                {
                    checkCurrentSeat.TableStatus = TableStatus.Empty;
                    checkCurrentSeat.UserId = null;
                    replaceSeat.UserId = SessionId;
                    replaceSeat.TableStatus = TableStatus.Occupied;
                    SaveChanges();
                    return user.Username + " , your seat has changed from T" + checkCurrentSeat.TableNo + " to T" + replaceSeat.TableNo;
                }
            }

            replaceSeat.UserId = SessionId;
            replaceSeat.TableStatus = TableStatus.Occupied;
            SaveChanges();

            return user.Username + " , your order is confirmed , your table seat is T" + replaceSeat.TableNo;

        }

        public int FoodCount(int SessionId)
        {
            var filterCount = OrderedFood(SessionId);
            int Count = 0;

            foreach (var item in filterCount)
            {
                Count += item.FoodQuantity;
            }
            return Count;
        }

        public int FoodPriceSum(int SessionId)
        {
            var filterCount = OrderedFood(SessionId);
            int TotalAmount = 0;
            foreach (var item in filterCount)
            {
                TotalAmount += item.TotalAmount;
            }
            return TotalAmount;
        }

        public IEnumerable<Table> GetEmptyTables()
        {
            return _context.Table.Where(d => d.TableStatus == TableStatus.Empty).ToList();
        }

        public IEnumerable<OrderCart> GetOrderCarts()
        {
            return _context.OrderCart.ToList();
        }

        public IEnumerable<OrderCart> OrderedFood(int SessionId)
        {
            return _context.OrderCart.Where(d => d.UserId == SessionId).ToList();
        }

        public void RemoveCart(OrderCart cart)
        {
            _context.OrderCart.Remove(cart);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateCart(OrderCart cart)
        {
            _context.Entry(cart).State = EntityState.Modified;
        }

        public Table CheckSeat(int SessionId)
        {
            var tableData = _context.Table.Where(d => d.UserId == SessionId).SingleOrDefault();
            return tableData;
        }

    }
}
