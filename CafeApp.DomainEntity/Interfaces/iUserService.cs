﻿

using CafeApp.DomainEntity.ViewModel;
using System.Collections.Generic;

namespace CafeApp.DomainEntity.Interfaces
{
    public interface iUserService
    {
        void CreateTables(User user);
        bool CheckDuplicateUser_EditMode(User user);
        bool CheckDuplicateUser(User user);
        void CreateAdmin();
        void SaveChanges();
        User CheckUserCredentials(LoginCredentialsViewModel user, Roles roles);
    }
}
