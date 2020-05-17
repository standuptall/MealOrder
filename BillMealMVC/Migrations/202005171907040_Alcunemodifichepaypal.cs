namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alcunemodifichepaypal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartHeads", "TransactionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartHeads", "TransactionId");
        }
    }
}
