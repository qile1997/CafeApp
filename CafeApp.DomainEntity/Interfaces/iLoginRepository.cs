using System;
using System.Collections.Generic;
using System.Text;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iLoginRepository
    {
        UserRoles Login(UserRoles userRoles);
    }
}
