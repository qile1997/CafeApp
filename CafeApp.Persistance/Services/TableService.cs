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
        private TableRepository _tableRepository = new TableRepository();
        private OrderCartService _orderCartService = new OrderCartService();
        public void ChangeTableStatus(Table table, int SessionId)
        {
            _tableRepository.GetTableById(table.TableId).UserId = null;
            _tableRepository.GetTableById(table.TableId).TableStatus = TableStatus.Empty;
            _orderCartService.ClearUserCartService(SessionId);
            _tableRepository.SaveChanges();
        }

        public void ReorderTables()
        {
            int i = 1;
            //Correct order of tables
            foreach (var item in _tableRepository.GetAllTables().ToList())
            {
                item.TableNo = i++;
            }

            _tableRepository.SaveChanges();
        }

    }
}
