namespace CafeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class i : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tables", "UserRolesId", "dbo.UserRoles");
            DropIndex("dbo.Tables", new[] { "UserRolesId" });
            AlterColumn("dbo.Tables", "UserRolesId", c => c.Int());
            CreateIndex("dbo.Tables", "UserRolesId");
            AddForeignKey("dbo.Tables", "UserRolesId", "dbo.UserRoles", "UserRolesId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tables", "UserRolesId", "dbo.UserRoles");
            DropIndex("dbo.Tables", new[] { "UserRolesId" });
            AlterColumn("dbo.Tables", "UserRolesId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tables", "UserRolesId");
            AddForeignKey("dbo.Tables", "UserRolesId", "dbo.UserRoles", "UserRolesId", cascadeDelete: true);
        }
    }
}
