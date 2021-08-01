using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApp.Persistance.Services
{
    public class TableService : iTableService
    {
        private CafeWebApp _context = new CafeWebApp();
        private TableRepository _tableRepository = new TableRepository();
        private OrderCartRepository _orderCartRepository = new OrderCartRepository();
        public void ChangeTableStatus(Table table, int SessionId)
        {
            table.UserId = null;
            table.TableStatus = TableStatus.Empty;
            _orderCartRepository.ClearCart(SessionId);
            _tableRepository.SaveChanges();
        }

        public void ReorderTables()
        {
            var TableCount = _tableRepository.GetAllTables();

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
            _tableRepository.SaveChanges();
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
