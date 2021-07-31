using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iFoodRepository
    {
        IEnumerable<Food> ReadAllFoods();
        Food GetFoodById(int? foodId);
        void CreateFood(Food food);
        void UpdateFood(Food food);
        void DeleteFood(Food food);
        void SaveChanges();
    }
}
