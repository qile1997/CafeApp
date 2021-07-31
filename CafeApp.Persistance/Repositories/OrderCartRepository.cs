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

        public string Cart(int Id, int SessionId)
        {
            //var checkId = Convert.ToInt32(Session["CustomerId"]);
            OrderCart cart = new OrderCart();

            //Filter cart/food ordered that belongs to the customerID
            var filterCart = db.OrderCart.Where(d => d.FoodsId == Id && d.UserRolesId == SessionId).SingleOrDefault();
            //Filter food that belongs to the customerID
            var filterFood = FilterFood(Id);

            if (filterCart != null)
            {
                filterCart.FoodQuantity++;
                filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                Save();
                return filterFood.FoodName + " added into quantity .";
            }
            else
            {
                cart.FoodsId = Id;
                cart.TotalAmount = filterFood.Price;
                cart.FoodQuantity = 1;
                cart.UserRolesId = SessionId;

                AddCart(cart);
                return filterFood.FoodName + " added into order .";
            }
        }

        public void CartQuantity(int FoodsId, string _operator, int SessionId)
        {
            var filterCart = db.OrderCart.Where(d => d.FoodsId == FoodsId && d.UserRolesId == SessionId).SingleOrDefault();
            var filterFood = FilterFood(FoodsId);
            if (_operator == "+")
            {
                filterCart.FoodQuantity++;
                filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                Save();
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
                    Save();
                    return;
                }
            }
        }
        public Food FilterFood(int Id)
        {
            return db.Foods.Where(d => d.FoodId == Id).SingleOrDefault();
        }

        public void ClearCart(int SessionId)
        {
            var orderedFood = OrderedFood(SessionId);
            foreach (var item in orderedFood)
            {
                db.OrderCart.Remove(item);
            }
            Save();
        }
        public void CancelOrder(int SessionId)
        {
            ClearCart(SessionId);
            var checkSeat = CheckSeat(SessionId);
            checkSeat.TableStatus = TableStatus.Empty;
            checkSeat.UserRolesId = null;
            Save();
        }

        public string ConfirmOrder(int SessionId, int Seat)
        {
            var user = db.UserRoles.Where(d => d.UserRolesId == SessionId).SingleOrDefault();
            var checkCurrentSeat = CheckSeat(SessionId);
            var replaceSeat = db.Table.Where(d => d.TableId == Seat).SingleOrDefault();
            var filterfood = OrderedFood(SessionId);

            if (checkCurrentSeat != null)
            {
                if (checkCurrentSeat.TableStatus == TableStatus.Occupied)
                {
                    checkCurrentSeat.TableStatus = TableStatus.Empty;
                    checkCurrentSeat.UserRolesId = null;
                    replaceSeat.UserRolesId = SessionId;
                    replaceSeat.TableStatus = TableStatus.Occupied;
                    Save();
                    return user.Username + " , your seat has changed from T" + checkCurrentSeat.TableNo + " to T" + replaceSeat.TableNo;
                }
            }

            replaceSeat.UserRolesId = SessionId;
            replaceSeat.TableStatus = TableStatus.Occupied;
            Save();

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
            return db.Table.Where(d => d.TableStatus == TableStatus.Empty).ToList();
        }

        public IEnumerable<OrderCart> GetOrderCarts()
        {
            return db.OrderCart.ToList();
        }

        public IEnumerable<OrderCart> OrderedFood(int SessionId)
        {
            return db.OrderCart.Where(d => d.UserRolesId == SessionId).ToList();
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

        public Table CheckSeat(int SessionId)
        {
            var tableData = db.Table.Where(d => d.UserRolesId == SessionId).SingleOrDefault();
            return tableData;
        }

    }
}
