using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using System.Data.Entity;
using CafeApp.Persistance.Services;

namespace CafeApp.Persistance.Repositories
{
    public class TableRepository : iTableRepository
    {
        private CafeWebApp _context = new CafeWebApp();
        public void AddTable(Table table)
        {
            _context.Table.Add(table);
            SaveChanges();
        }

        public void DeleteAllTables()
        {
            _context.Table.RemoveRange(GetAllTables());
            SaveChanges();
        }

        public void DeleteTable(Table table)
        {
            _context.Table.Remove(table);
            SaveChanges();
        }

        public IEnumerable<Table> GetAllTables()
        {
            return _context.Table.OrderBy(d => d.TableNo).ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Table GetTableById(int? id)
        {
            return _context.Table.Find(id);
        }
        public void UpdateTable(Table table)
        {
            _context.Entry(table).State = EntityState.Modified;
            SaveChanges();
        }
        public void CreateTableWithOneClick()
        {
            Table table = new Table();

            //Order table Id , grab last table and increase Id by 1
            table.TableNo = _context.Table.OrderBy(d => d.TableNo).ToList().Last().TableNo + 1;
            table.TableStatus = TableStatus.Empty;
            AddTable(table);
        }
    }
}
