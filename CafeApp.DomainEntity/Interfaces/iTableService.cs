using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApp.DomainEntity.Interfaces
{
   public interface iTableService
    {
        void ChangeTableStatus(Table table, int SessionId);
        void ReorderTables();
    }
}
