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
                UserCart.TotalAmount = GetUserFoodBySessionId(FoodId).Price * UserCart.FoodQuantity;
                SaveChanges();
                return GetUserFoodBySessionId(FoodId).FoodName + " added into quantity .";
            }
            else
            {
                OrderCart orderCart = new OrderCart()
                {
                    FoodsId = FoodId,
                    TotalAmount = GetUserFoodBySessionId(FoodId).Price,
                    FoodQuantity = 1,
                    UserId = SessionId
                };

                _orderCartRepository.AddOrderCart(orderCart);
                return GetUserFoodBySessionId(FoodId).FoodName + " added into order .";
            }
        }

        public void UserOrderCartQuantityService(int FoodsId, string Operator, int SessionId)
        {
            var UserCart = _context.OrderCart.Where(d => d.FoodsId == FoodsId && d.UserId == SessionId).SingleOrDefault();

            if (Operator == "+")
            {
                UserCart.FoodQuantity++;
                UserCart.TotalAmount = GetUserFoodBySessionId(FoodsId).Price * UserCart.FoodQuantity;
                SaveChanges();
            }
            else if (Operator == "x")
            {
                _orderCartRepository.RemoveOrderCart(UserCart);
            }
            else
            {
                if (UserCart.FoodQuantity > 0)
                {
                    UserCart.FoodQuantity--;
                    if (UserCart.FoodQuantity == 0)
                    {
                        _orderCartRepository.RemoveOrderCart(UserCart);
                    }
                    UserCart.TotalAmount = GetUserFoodBySessionId(FoodsId).Price * UserCart.FoodQuantity;
                    SaveChanges();
                }
            }
        }

        public void ClearUserCartService(int SessionId)
        {
            _context.OrderCart.RemoveRange(_context.OrderCart.Where(d => d.UserId == SessionId).ToList());
            SaveChanges();
        }

        public void CancelUserOrderService(int SessionId)
        {
            //Remove Table and clear order cart
            ClearUserCartService(SessionId);
            GetUserTableBySessionId(SessionId).TableStatus = TableStatus.Empty;
            GetUserTableBySessionId(SessionId).UserId = null;
            SaveChanges();
        }

        public string ConfirmUserOrderService(int SessionId, int Seat)
        {
            var User = _context.Users.Where(d => d.UserId == SessionId).SingleOrDefault();
            var NewSeat = _context.Table.Where(d => d.TableId == Seat).SingleOrDefault();

            if (GetUserTableBySessionId(SessionId) != null && GetUserTableBySessionId(SessionId).TableStatus == TableStatus.Occupied)
            {
                GetUserTableBySessionId(SessionId).TableStatus = TableStatus.Empty;
                GetUserTableBySessionId(SessionId).UserId = null;
                NewSeat.UserId = SessionId;
                NewSeat.TableStatus = TableStatus.Occupied;
                SaveChanges();
                return User.Username + " , your seat has changed from T" + GetUserTableBySessionId(SessionId).TableNo + " to T" + NewSeat.TableNo;
            }

            NewSeat.UserId = SessionId;
            NewSeat.TableStatus = TableStatus.Occupied;
            SaveChanges();

            return User.Username + " , your order is confirmed , your table seat is T" + NewSeat.TableNo;
        }

        public int GetFoodQuantityBySessionId(int SessionId)
        {
            return _context.OrderCart.Where(x => x.UserId == SessionId).ToList().Sum(x => x.FoodQuantity);
        }

        public int GetFoodPriceTotalAmountBySessionId(int SessionId)
        {
            return _context.OrderCart.Where(d => d.UserId == SessionId).ToList().Sum(x => x.TotalAmount);
        }

        public IEnumerable<OrderCart> UserOrderCart(int SessionId)
        {
            return _context.OrderCart.Where(d => d.UserId == SessionId).ToList();
        }

        public Table GetUserTableBySessionId(int SessionId)
        {
            return _context.Table.Where(d => d.UserId == SessionId).SingleOrDefault();
        }
        public Food GetUserFoodBySessionId(int SessionId)
        {
            return _context.Foods.Where(d => d.FoodId == SessionId).SingleOrDefault();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
