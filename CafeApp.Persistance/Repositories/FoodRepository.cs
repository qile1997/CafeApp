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
        private CafeWebApp _context;
        public FoodRepository(CafeWebApp context)
        {
            _context = context;
        }
        public void CreateFood(Food food)
        {
            _context.Foods.Add(food);
            SaveChanges();
        }

        public void DeleteFood(Food food)
        {
            _context.Foods.Remove(food);
            SaveChanges();
        }

        public Food GetFoodById(int? id)
        {
            Food food = _context.Foods.Find(id);
            return food;
        }

        public IEnumerable<Food> ReadAllFoods()
        {
            return _context.Foods.ToList();
        }

        public void UpdateFood(Food food)
        {
            _context.Entry(food).State = EntityState.Modified;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
