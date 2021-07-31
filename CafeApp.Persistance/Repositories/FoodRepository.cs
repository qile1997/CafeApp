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
    public class FoodRepository : iFoodRepository
    {
        private CafeWebApp db = new CafeWebApp();
        public void CreateFood(Food food)
        {
            db.Foods.Add(food);
            SaveChanges();
        }

        public void DeleteFood(Food food)
        {
            db.Foods.Remove(food);
            SaveChanges();
        }

        public Food GetFoodById(int? id)
        {
            Food food = db.Foods.Find(id);
            return food;
        }

        public IEnumerable<Food> ReadAllFoods()
        {
            return db.Foods.ToList();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void UpdateFood(Food food)
        {
            db.Entry(food).State = EntityState.Modified;
        }
    }
}
