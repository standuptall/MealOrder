namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class menuprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuRows", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuRows", "Price");
        }
    }
}
