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
        int GetFoodQuantityBySessionId(int SessionId);
        int GetFoodPriceTotalAmountBySessionId(int SessionId);
        void UserOrderCartQuantityService(int FoodId, string Operator, int SessionId);
        string UserOrderCartService(int FoodId, int SessionId);
        string ConfirmUserOrderService(int SessionId, int Seat);
        void ClearUserCartService(int SessionId);
        Food GetUserFoodBySessionId(int SessionId);
        Table GetUserTableBySessionId(int SessionId);
        IEnumerable<OrderCart> UserOrderCart(int FoodId);
        void SaveChanges();
    }
}
