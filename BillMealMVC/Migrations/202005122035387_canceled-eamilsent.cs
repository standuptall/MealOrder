namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class canceledeamilsent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartHeads", "Canceled", c => c.Boolean(nullable: false));
            AddColumn("dbo.CartHeads", "EmailSent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartHeads", "EmailSent");
            DropColumn("dbo.CartHeads", "Canceled");
        }
    }
}
