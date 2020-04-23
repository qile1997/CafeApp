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
        private OrderCartRepository orderRepo = new OrderCartRepository();
        public void AddTable(Table table)
        {
            db.Table.Add(table);
            Save();
        }

        public void DeleteAllTable()
        {
            var AllTables = GetTables();
            db.Table.RemoveRange(AllTables);
            Save();
        }

        public void DeleteTable(Table table)
        {
            db.Table.Remove(table);
            Save();
        }

        public IEnumerable<Table> GetTables()
        {
            return db.Table.OrderBy(d => d.TableNo).ToList();
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
        public void ChangeStatus(Table table, int SessionId)
        {
            table.UserRolesId = null;
            table.TableStatus = TableStatus.Empty;
            orderRepo.ClearCart(SessionId);
            Save();
        }
        public void UpdateTable(Table table)
        {
            db.Entry(table).State = EntityState.Modified;
            Save();
        }

        public void ReorderTables()
        {
            var TableCount = GetTables();

            //Correct order of tables
            List<int> arr = new List<int>();

            for (int x = 11; x <= TableCount.Count(); x++)
            {
                arr.Add(x);
            }

            int[] _arr = arr.ToArray();

            var reordertable = db.Table.OrderByDescending(d => d.TableNo).Take(arr.Count()).ToList();
            var updateReorder = reordertable.OrderBy(d => d.TableNo).ToList();

            int i = 0;

            foreach (var item in updateReorder)
            {
                item.TableNo = _arr[i];
                i++;
                Save();
            }
        }

        public void CreateTable()
        {
            var tableList = db.Table.OrderBy(d => d.TableNo).Skip(10).ToList();
            Table table = new Table();

            if (tableList.Count() < 1)
            {
                table.TableNo = 11;
                table.TableStatus = TableStatus.Empty;
                AddTable(table);
            }
            else
            {
                var grabLast = db.Table.OrderBy(d => d.TableNo).ToList().Last();

                table.TableNo = grabLast.TableNo + 1;
                table.TableStatus = TableStatus.Empty;
                AddTable(table);
            }
        }

        public bool GetTableStatus()
        {
            var check = db.Table.GroupBy(d => d.TableStatus == TableStatus.Occupied).Count();

            if (check > 1)
            {
                return true;
            }
            return false;
        }
    }
}
