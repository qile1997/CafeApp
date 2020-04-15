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

        public string Cart(int Id, int checkId)
        {
            //var checkId = Convert.ToInt32(Session["CustomerId"]);
            OrderCart cart = new OrderCart();

            //Filter cart/food ordered that belongs to the customerID
            var filterCart = db.OrderCart.Where(d => d.FoodsId == Id && d.UserRolesId == checkId).SingleOrDefault();
            //Filter food that belongs to the customerID
            var filterFood = FilterFood(Id);

            if (filterCart != null)
            {
                filterCart.FoodQuantity++;
                filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                Save();
                return filterFood.FoodName + " added into quantity order";
            }
            else
            {
                cart.FoodsId = Id;
                cart.TotalAmount = filterFood.Price;
                cart.FoodQuantity = 1;
                cart.UserRolesId = checkId;

                AddCart(cart);
                return filterFood.FoodName + " added into order";
            }
        }

        public string CartQuantity(int Id, string _operator)
        {
            var filterCart = db.OrderCart.Where(d => d.FoodsId == Id).SingleOrDefault();
            var filterFood = FilterFood(Id);
            if (_operator == "+")
            {
                filterCart.FoodQuantity++;
                filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                Save();
                return "Quantity Updated";
            }
            else if (_operator == "x")
            {
                RemoveCart(filterCart);
                return "Food Removed";
            }
            else
            {
                if (filterCart.FoodQuantity > 0)
                {
                    filterCart.FoodQuantity--;
                    if (filterCart.FoodQuantity == 0)
                    {
                        RemoveCart(filterCart);
                        return "Food Removed";
                    }
                    filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                    Save();
                    return "Quantity Deleted";
                }
            }
            return "";
        }

        public string ClearCart()
        {
            foreach (var item in db.OrderCart)
            {
                db.OrderCart.Remove(item);
            }
            Save();
            return "Cart cleared succesfully";
        }

        public string ConfirmOrder(int checkId, int Seat)
        {
            var checkSeat = db.Table.Where(d => d.UserRolesId == checkId).SingleOrDefault();
            var replaceSeat = db.Table.Where(d => d.TableId == Seat).SingleOrDefault();
            var filterfood = OrderedFood(checkId);

            if (filterfood.Count() < 1)
            {
                return "No food in order cart. Cannot confirm order";
            }
            else
            {
                if (checkSeat != null)
                {
                    return "Your order is already confirmed, your table seat is ";
                }

                replaceSeat.UserRolesId = checkId;
                replaceSeat.TableStatus = TableStatus.Occupied;
                Save();

                return "Order confirmed, your table seat is " + replaceSeat.TableNo;
            }
        }

        public Foods FilterFood(int Id)
        {
            return db.Foods.Where(d => d.FoodsId == Id).SingleOrDefault();
        }

        public int FoodCount(int checkId)
        {
            var filterCount = OrderedFood(checkId);
            int Count = 0;

            foreach (var item in filterCount)
            {
                Count += item.FoodQuantity;
            }
            return Count;
        }

        public int FoodPriceSum(int checkId)
        {
            var filterCount = OrderedFood(checkId);
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

        public IEnumerable<OrderCart> OrderedFood(int checkId)
        {
            return db.OrderCart.Where(d => d.UserRolesId == checkId).ToList();
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
