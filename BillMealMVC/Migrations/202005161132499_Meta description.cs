namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Metadescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "MetaDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "MetaDescription");
        }
    }
}
