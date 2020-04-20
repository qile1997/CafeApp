using CafeApp.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApp.DomainEntity1.Interfaces
{
    public interface iMethodRepository
    {
        //Login
        UserRoles Login(UserRoles userRoles);
        //Order Cart 
        IEnumerable<Table> GetEmptyTables();
        void CancelOrder(int id);
        int FoodCount(int id);
        int FoodPriceSum(int id);
        void CartQuantity(int id, string _operator);
        string Cart(int id, int checkId);
        string ConfirmOrder(int id, int Seat);
        IEnumerable<OrderCart> OrderedFood(int id);
        //

    }
}
