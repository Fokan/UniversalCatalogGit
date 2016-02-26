namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrSecond1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Properties", "Type_Id", "dbo.Types");
            DropIndex("dbo.Properties", new[] { "Type_Id" });
            CreateTable(
                "dbo.PropertyTypes",
                c => new
                    {
                        Property_Id = c.Guid(nullable: false),
                        Type_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Property_Id, t.Type_Id })
                .ForeignKey("dbo.Properties", t => t.Property_Id, cascadeDelete: true)
                .ForeignKey("dbo.Types", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.Property_Id)
                .Index(t => t.Type_Id);
            
            DropColumn("dbo.Properties", "Type_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Properties", "Type_Id", c => c.Guid());
            DropForeignKey("dbo.PropertyTypes", "Type_Id", "dbo.Types");
            DropForeignKey("dbo.PropertyTypes", "Property_Id", "dbo.Properties");
            DropIndex("dbo.PropertyTypes", new[] { "Type_Id" });
            DropIndex("dbo.PropertyTypes", new[] { "Property_Id" });
            DropTable("dbo.PropertyTypes");
            CreateIndex("dbo.Properties", "Type_Id");
            AddForeignKey("dbo.Properties", "Type_Id", "dbo.Types", "Id");
        }
    }
}
