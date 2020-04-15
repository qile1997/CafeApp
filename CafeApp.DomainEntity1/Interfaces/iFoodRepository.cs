using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iFoodRepository
    {
        IEnumerable<Foods> GetFoods();
        Foods food(int? id);
        void AddFood(Foods food);
        void UpdateFood(Foods food);
        void DeleteFood(Foods food);
        void Save();
    }
}
