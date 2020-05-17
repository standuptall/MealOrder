namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paypal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartHeads", "TotalPaid", c => c.Double(nullable: false));
            AddColumn("dbo.Settings", "PagaOnline", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "PagaOnline");
            DropColumn("dbo.CartHeads", "TotalPaid");
        }
    }
}
