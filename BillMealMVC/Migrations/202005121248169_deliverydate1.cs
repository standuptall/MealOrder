namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deliverydate1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CartHeads", "DeliveryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CartHeads", "DeliveryDate", c => c.DateTime(nullable: false));
        }
    }
}
