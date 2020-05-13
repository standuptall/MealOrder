namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppName = c.String(),
                        UserEmail = c.String(),
                        PasswordEmail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Settings");
        }
    }
}
