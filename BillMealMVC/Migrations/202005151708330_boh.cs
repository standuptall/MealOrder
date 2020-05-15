namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boh : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Settings", "FooterDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "FooterDescription", c => c.String());
        }
    }
}
