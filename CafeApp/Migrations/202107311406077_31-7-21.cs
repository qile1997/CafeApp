namespace CafeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _31721 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        FoodId = c.Int(nullable: false, identity: true),
                        FoodCategory = c.Int(nullable: false),
                        FoodName = c.String(),
                        PhotoFile = c.String(),
                        Price = c.Int(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.FoodId);
            
            CreateTable(
                "dbo.OrderCarts",
                c => new
                    {
                        OrderCartId = c.Int(nullable: false, identity: true),
                        FoodsId = c.Int(nullable: false),
                        FoodQuantity = c.Int(nullable: false),
                        TotalAmount = c.Int(nullable: false),
                        UserRolesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderCartId)
                .ForeignKey("dbo.Foods", t => t.FoodsId, cascadeDelete: true)
                .ForeignKey("dbo.UserRoles", t => t.UserRolesId, cascadeDelete: true)
                .Index(t => t.FoodsId)
                .Index(t => t.UserRolesId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserRolesId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Roles = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserRolesId);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        TableId = c.Int(nullable: false, identity: true),
                        TableNo = c.Int(nullable: false),
                        UserRolesId = c.Int(),
                        TableStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TableId)
                .ForeignKey("dbo.UserRoles", t => t.UserRolesId)
                .Index(t => t.UserRolesId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderCarts", "UserRolesId", "dbo.UserRoles");
            DropForeignKey("dbo.Tables", "UserRolesId", "dbo.UserRoles");
            DropForeignKey("dbo.OrderCarts", "FoodsId", "dbo.Foods");
            DropIndex("dbo.Tables", new[] { "UserRolesId" });
            DropIndex("dbo.OrderCarts", new[] { "UserRolesId" });
            DropIndex("dbo.OrderCarts", new[] { "FoodsId" });
            DropTable("dbo.Tables");
            DropTable("dbo.UserRoles");
            DropTable("dbo.OrderCarts");
            DropTable("dbo.Foods");
        }
    }
}
