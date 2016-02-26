namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrSecond : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Descriptions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Value = c.String(nullable: false),
                        Property_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Properties", t => t.Property_Id)
                .Index(t => t.Property_Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Type_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Types", t => t.Type_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.Binary(),
                        Name = c.String(),
                        Item_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        ParentType_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Types", t => t.ParentType_Id)
                .Index(t => t.ParentType_Id);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Type_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Types", t => t.Type_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.ItemDescriptions",
                c => new
                    {
                        Item_Id = c.Guid(nullable: false),
                        Description_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Item_Id, t.Description_Id })
                .ForeignKey("dbo.Items", t => t.Item_Id, cascadeDelete: true)
                .ForeignKey("dbo.Descriptions", t => t.Description_Id, cascadeDelete: true)
                .Index(t => t.Item_Id)
                .Index(t => t.Description_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Properties", "Type_Id", "dbo.Types");
            DropForeignKey("dbo.Descriptions", "Property_Id", "dbo.Properties");
            DropForeignKey("dbo.Types", "ParentType_Id", "dbo.Types");
            DropForeignKey("dbo.Items", "Type_Id", "dbo.Types");
            DropForeignKey("dbo.Images", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.ItemDescriptions", "Description_Id", "dbo.Descriptions");
            DropForeignKey("dbo.ItemDescriptions", "Item_Id", "dbo.Items");
            DropIndex("dbo.ItemDescriptions", new[] { "Description_Id" });
            DropIndex("dbo.ItemDescriptions", new[] { "Item_Id" });
            DropIndex("dbo.Properties", new[] { "Type_Id" });
            DropIndex("dbo.Types", new[] { "ParentType_Id" });
            DropIndex("dbo.Images", new[] { "Item_Id" });
            DropIndex("dbo.Items", new[] { "Type_Id" });
            DropIndex("dbo.Descriptions", new[] { "Property_Id" });
            DropTable("dbo.ItemDescriptions");
            DropTable("dbo.Properties");
            DropTable("dbo.Types");
            DropTable("dbo.Images");
            DropTable("dbo.Items");
            DropTable("dbo.Descriptions");
        }
    }
}
