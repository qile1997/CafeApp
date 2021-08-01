using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApp.Persistance.Services
{
    public class OrderCartService : iOrderCartService
    {
        private CafeWebApp _context = new CafeWebApp();
        private OrderCartRepository _orderCartRepository = new OrderCartRepository();
        public string UserOrderCartService(int FoodId, int SessionId)
        {
            //Filter cart/food ordered that belongs to the customerID
            var UserCart = _context.OrderCart.Where(d => d.FoodsId == FoodId && d.UserId == SessionId).SingleOrDefault();
            //Filter food that belongs to the customerID

            if (UserCart != null)
            {
                UserCart.FoodQuantity++;
                UserCart.TotalAmount = UserOrderedFood(FoodId).Price * UserCart.FoodQuantity;
                SaveChanges();
                return UserOrderedFood(FoodId).FoodName + " added into quantity .";
            }
            else
            {
                OrderCart orderCart = new OrderCart()
                {
                    FoodsId = FoodId,
                    TotalAmount = UserOrderedFood(FoodId).Price,
                    FoodQuantity = 1,
                    UserId = SessionId
                };

                _orderCartRepository.AddOrderCart(orderCart);
                return UserOrderedFood(FoodId).FoodName + " added into order .";
            }
        }

        public void UserOrderCartQuantityService(int FoodsId, string Operator, int SessionId)
        {
            var UserCart = _context.OrderCart.Where(d => d.FoodsId == FoodsId && d.UserId == SessionId).SingleOrDefault();

            if (Operator == "+")
            {
                UserCart.FoodQuantity++;
                UserCart.TotalAmount = UserOrderedFood(FoodsId).Price * UserCart.FoodQuantity;
                SaveChanges();
                return;
            }
            else if (Operator == "x")
            {
                _orderCartRepository.RemoveOrderCart(UserCart);
                return;
            }
            else
            {
                if (UserCart.FoodQuantity > 0)
                {
                    UserCart.FoodQuantity--;
                    if (UserCart.FoodQuantity == 0)
                    {
                        _orderCartRepository.RemoveOrderCart(UserCart);
                        return;
                    }
                    UserCart.TotalAmount = UserOrderedFood(FoodsId).Price * UserCart.FoodQuantity;
                    SaveChanges();
                    return;
                }
            }
        }
        public Food UserOrderedFood(int SessionId)
        {
            return _context.Foods.Where(d => d.FoodId == SessionId).SingleOrDefault();
        }

        public void ClearUserCartService(int SessionId)
        {
            var orderedFood = UserOrderedFoodList(SessionId);
            foreach (var item in orderedFood)
            {
                _context.OrderCart.Remove(item);
            }
            SaveChanges();
        }

        public void CancelUserOrderService(int SessionId)
        {
            ClearUserCartService(SessionId);
            CheckUserTable(SessionId).TableStatus = TableStatus.Empty;
            CheckUserTable(SessionId).UserId = null;
            SaveChanges();
        }

        public string ConfirmUserOrderService(int SessionId, int Seat)
        {
            var User = _context.Users.Where(d => d.UserId == SessionId).SingleOrDefault();
            var NewSeat = _context.Table.Where(d => d.TableId == Seat).SingleOrDefault();

            if (CheckUserTable(SessionId) != null && CheckUserTable(SessionId).TableStatus == TableStatus.Occupied)
            {
                CheckUserTable(SessionId).TableStatus = TableStatus.Empty;
                CheckUserTable(SessionId).UserId = null;
                NewSeat.UserId = SessionId;
                NewSeat.TableStatus = TableStatus.Occupied;
                SaveChanges();
                return User.Username + " , your seat has changed from T" + CheckUserTable(SessionId).TableNo + " to T" + NewSeat.TableNo;
            }

            NewSeat.UserId = SessionId;
            NewSeat.TableStatus = TableStatus.Occupied;
            SaveChanges();

            return User.Username + " , your order is confirmed , your table seat is T" + NewSeat.TableNo;

        }

        public int FoodCount(int SessionId)
        {
            int Count = 0;

            foreach (var item in UserOrderedFoodList(SessionId))
            {
                Count += item.FoodQuantity;
            }

            return Count;
        }

        public int FoodPriceSum(int SessionId)
        {
            var filterCount = UserOrderedFoodList(SessionId);
            int TotalAmount = 0;
            foreach (var item in filterCount)
            {
                TotalAmount += item.TotalAmount;
            }
            return TotalAmount;
        }

        public IEnumerable<OrderCart> UserOrderedFoodList(int SessionId)
        {
            return _context.OrderCart.Where(d => d.UserId == SessionId).ToList();
        }

        public Table CheckUserTable(int SessionId)
        {
            return _context.Table.Where(d => d.UserId == SessionId).SingleOrDefault();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
