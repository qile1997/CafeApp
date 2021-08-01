using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iTableRepository
    {
        IEnumerable<Table> GetAllTables();
        Table GetTableById(int? id);
        void CreateTableWithOneClick();
        void AddTable(Table table);
        void UpdateTable(Table table);
        void DeleteTable(Table table);
        void DeleteAllTables();
        void SaveChanges();
    }
}
