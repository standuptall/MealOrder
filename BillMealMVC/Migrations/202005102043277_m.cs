namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartHeads",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        meal_id = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CartId);
            
            CreateTable(
                "dbo.CartRows",
                c => new
                    {
                        CartRowId = c.Int(nullable: false, identity: true),
                        CartId = c.Int(),
                        ItemId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CartRowId)
                .ForeignKey("dbo.CartHeads", t => t.CartId)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.CartId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CategoryId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        DiscountAmount = c.Double(nullable: false),
                        DiscountPercent = c.Double(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.ItemCategories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.ItemCategories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartRows", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "CategoryId", "dbo.ItemCategories");
            DropForeignKey("dbo.CartRows", "CartId", "dbo.CartHeads");
            DropIndex("dbo.Items", new[] { "CategoryId" });
            DropIndex("dbo.CartRows", new[] { "ItemId" });
            DropIndex("dbo.CartRows", new[] { "CartId" });
            DropTable("dbo.ItemCategories");
            DropTable("dbo.Items");
            DropTable("dbo.CartRows");
            DropTable("dbo.CartHeads");
        }
    }
}
