namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class footer1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "FooterDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "FooterDescription");
        }
    }
}
