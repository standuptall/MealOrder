namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deliverydate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartHeads", "DeliveryDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.CartHeads", "DeliveryHour");
            DropColumn("dbo.CartHeads", "DeliveryMinute");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartHeads", "DeliveryMinute", c => c.Int(nullable: false));
            AddColumn("dbo.CartHeads", "DeliveryHour", c => c.Int(nullable: false));
            DropColumn("dbo.CartHeads", "DeliveryDate");
        }
    }
}
