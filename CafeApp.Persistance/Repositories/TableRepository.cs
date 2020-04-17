using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using System.Data.Entity;

namespace CafeApp.Persistance.Repositories
{
    public class TableRepository : iTableRepository
    {
        private CafeWebApp db = new CafeWebApp();
        public void AddTable(Table table)
        {
            db.Table.Add(table);
            Save();
        }

        public void DeleteTable(Table table)
        {
            db.Table.Remove(table);
            Save();
        }

        public IEnumerable<Table> GetTables()
        {
            return db.Table.ToList();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public Table table(int? id)
        {
            Table table = db.Table.Find(id);
            return table;
        }

        public void UpdateTable(Table table)
        {
            db.Entry(table).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
