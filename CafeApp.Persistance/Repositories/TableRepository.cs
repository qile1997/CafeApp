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
        private CafeWebApp _context;
        public TableRepository(CafeWebApp context)
        {
            _context = context;
        }
        private OrderCartRepository orderRepo = new OrderCartRepository();

        public void AddTable(Table table)
        {
            _context.Table.Add(table);
            Save();
        }

        public void DeleteAllTable()
        {
            var AllTables = GetTables();
            _context.Table.RemoveRange(AllTables);
            Save();
        }

        public void DeleteTable(Table table)
        {
            _context.Table.Remove(table);
            Save();
        }

        public IEnumerable<Table> GetTables()
        {
            return _context.Table.OrderBy(d => d.TableNo).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Table table(int? id)
        {
            return _context.Table.Find(id);
        }
        public void ChangeStatus(Table table, int SessionId)
        {
            table.UserRolesId = null;
            table.TableStatus = TableStatus.Empty;
            orderRepo.ClearCart(SessionId);
            Save();
        }
        public void UpdateTable(Table table)
        {
            _context.Entry(table).State = EntityState.Modified;
            Save();
        }

        public void ReorderTables()
        {
            var TableCount = GetTables();

            //Correct order of tables
            List<int> arr = new List<int>();

            for (int x = 1; x <= TableCount.Count(); x++)
            {
                arr.Add(x);
            }

            int[] _arr = arr.ToArray();

            var tables = _context.Table.OrderBy(d => d.TableNo).ToList();

            int i = 0;

            foreach (var item in tables)
            {
                item.TableNo = _arr[i];
                i++;
            }
            Save();
        }

        public void CreateTable()
        {
            Table table = new Table();

            var grabLast = _context.Table.OrderBy(d => d.TableNo).ToList().Last();

            table.TableNo = grabLast.TableNo + 1;
            table.TableStatus = TableStatus.Empty;
            AddTable(table);
        }

        public bool GetTableStatus()
        {
            var check = _context.Table.GroupBy(d => d.TableStatus == TableStatus.Occupied).Count();

            if (check < 2)
            {
                return true;
            }
            return false;
        }
    }
}
