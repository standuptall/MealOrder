namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationpath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "ApplicationPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "ApplicationPath");
        }
    }
}
