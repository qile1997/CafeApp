using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;

namespace CafeApp.Persistance.Repositories
{
    public class TableRepository : iTableRepository
    {
        private CafeWebApp db = new CafeWebApp();
        public void AddTable(Table table)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Table> GetTables()
        {
            return
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Table table(int? id)
        {
            throw new NotImplementedException();
        }

        public void UpdateTable(Table table)
        {
            throw new NotImplementedException();
        }
    }
}
