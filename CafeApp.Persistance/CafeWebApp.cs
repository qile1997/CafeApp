using CafeApp.DomainEntity;
using System.Data.Entity;

namespace CafeApp.Persistance
{
    public class CafeWebApp : DbContext
    {
        // Your context has been configured to use a 'CafeWebApp' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CafeApp.Models.CafeWebApp' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'CafeWebApp' 
        // connection string in the application configuration file.
        public CafeWebApp()
            : base("name=CafeWebApp")
        {
        }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Foods> Foods { get; set; }
        public DbSet<OrderCart> OrderCart { get; set; }
        public DbSet<Table> Table { get; set; }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}