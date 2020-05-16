namespace BillMealMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MenuHeadsAndRows : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuHeads",
                c => new
                    {
                        MenuHeadId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.MenuHeadId);
            
            CreateTable(
                "dbo.MenuRows",
                c => new
                    {
                        MenuRowId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        MenuHead_MenuHeadId = c.Int(),
                    })
                .PrimaryKey(t => t.MenuRowId)
                .ForeignKey("dbo.MenuHeads", t => t.MenuHead_MenuHeadId)
                .Index(t => t.MenuHead_MenuHeadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuRows", "MenuHead_MenuHeadId", "dbo.MenuHeads");
            DropIndex("dbo.MenuRows", new[] { "MenuHead_MenuHeadId" });
            DropTable("dbo.MenuRows");
            DropTable("dbo.MenuHeads");
        }
    }
}
