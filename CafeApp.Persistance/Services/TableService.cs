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
        private OrderCartService _orderCartService = new OrderCartService();
        public void ChangeTableStatus(Table table, int SessionId)
        {
            var removeTable = _tableRepository.GetTableById(table.TableId);
            removeTable.UserId = null;
            removeTable.TableStatus = TableStatus.Empty;
            _orderCartService.ClearUserCartService(SessionId);
            _tableRepository.SaveChanges();
        }

        public void ReorderTables()
        {
            //Correct order of tables
            List<int> arr = new List<int>();

            for (int x = 1; x <= _tableRepository.GetAllTables().Count(); x++)
            {
                arr.Add(x);
            }

            int[] _arr = arr.ToArray();

            int i = 0;

            foreach (var item in _context.Table.OrderBy(d => d.TableNo).ToList())
            {
                item.TableNo = _arr[i];
                i++;
            }
            _tableRepository.SaveChanges();
        }

        public bool GetTableStatus()
        {
            return _context.Table.GroupBy(d => d.TableStatus == TableStatus.Occupied).Count() < 2 ? true : false;
        }
    }
}
