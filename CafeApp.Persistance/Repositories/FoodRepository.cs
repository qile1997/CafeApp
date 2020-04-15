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
        public void AddFood(Foods food)
        {
            db.Foods.Add(food);
            Save();
        }

        public void DeleteFood(Foods food)
        {
            db.Foods.Remove(food);
            Save();
        }

        public Foods food(int? id)
        {
            Foods food = db.Foods.Find(id);
            return food;
        }

        public IEnumerable<Foods> GetFoods()
        {
            return db.Foods.ToList();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateFood(Foods food)
        {
            db.Entry(food).State = EntityState.Modified;
        }
    }
}
