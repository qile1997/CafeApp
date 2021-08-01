using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iOrderCartService
    {
        void CancelUserOrderService(int SessionId);
        int FoodCount(int SessionId);
        int FoodPriceSum(int SessionId);
        void UserOrderCartQuantityService(int FoodId, string Operator, int SessionId);
        string UserOrderCartService(int FoodId, int SessionId);
        string ConfirmUserOrderService(int SessionId, int Seat);
        void ClearUserCartService(int SessionId);
        Food UserOrderedFood(int SessionId);
        Table CheckUserTable(int SessionId);
        IEnumerable<OrderCart> UserOrderedFoodList(int FoodId);
        void SaveChanges();
    }
}
