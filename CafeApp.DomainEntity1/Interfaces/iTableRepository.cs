using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iTableRepository
    {
        IEnumerable<Table> GetTables();
        Table table(int? id);
        void AddTable(Table table);
        void UpdateTable(Table table);
        void DeleteTable(Table table);
        void DeleteAllTable();
        void ChangeStatus(Table table,int SessionId);
        void ReorderTables();
        void CreateTable();
        bool GetTableStatus();
        void Save();
    }
}
