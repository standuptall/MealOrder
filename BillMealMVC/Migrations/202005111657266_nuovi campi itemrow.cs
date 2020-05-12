namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuovicampiitemrow : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CartRows", "ItemId", "dbo.Items");
            DropIndex("dbo.CartRows", new[] { "ItemId" });
            AddColumn("dbo.CartRows", "ItemName", c => c.String());
            AddColumn("dbo.CartRows", "ItemDescription", c => c.String());
            AddColumn("dbo.CartRows", "ItemPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.CartRows", "ItemId", c => c.Int());
            CreateIndex("dbo.CartRows", "ItemId");
            AddForeignKey("dbo.CartRows", "ItemId", "dbo.Items", "ItemId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartRows", "ItemId", "dbo.Items");
            DropIndex("dbo.CartRows", new[] { "ItemId" });
            AlterColumn("dbo.CartRows", "ItemId", c => c.Int(nullable: false));
            DropColumn("dbo.CartRows", "ItemPrice");
            DropColumn("dbo.CartRows", "ItemDescription");
            DropColumn("dbo.CartRows", "ItemName");
            CreateIndex("dbo.CartRows", "ItemId");
            AddForeignKey("dbo.CartRows", "ItemId", "dbo.Items", "ItemId", cascadeDelete: true);
        }
    }
}
