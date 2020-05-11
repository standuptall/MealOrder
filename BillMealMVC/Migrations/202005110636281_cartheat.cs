namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cartheat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartHeads", "Closed", c => c.Boolean(nullable: false));
            AddColumn("dbo.CartHeads", "ClosedDate", c => c.DateTime());
            AddColumn("dbo.CartHeads", "Notes", c => c.String());
            AddColumn("dbo.CartHeads", "Email", c => c.String());
            AddColumn("dbo.CartHeads", "Phone", c => c.String());
            AddColumn("dbo.CartHeads", "DeliveryHour", c => c.Int(nullable: false));
            AddColumn("dbo.CartHeads", "DeliveryMinute", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartHeads", "DeliveryMinute");
            DropColumn("dbo.CartHeads", "DeliveryHour");
            DropColumn("dbo.CartHeads", "Phone");
            DropColumn("dbo.CartHeads", "Email");
            DropColumn("dbo.CartHeads", "Notes");
            DropColumn("dbo.CartHeads", "ClosedDate");
            DropColumn("dbo.CartHeads", "Closed");
        }
    }
}
