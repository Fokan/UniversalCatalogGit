namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr35 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ImageItems", "Image_Id", "dbo.Images");
            DropForeignKey("dbo.ImageItems", "Item_Id", "dbo.Items");
            DropIndex("dbo.ImageItems", new[] { "Image_Id" });
            DropIndex("dbo.ImageItems", new[] { "Item_Id" });
            AddColumn("dbo.Images", "Item_Id", c => c.Guid());
            AddColumn("dbo.Types", "Image_Id", c => c.Guid());
            CreateIndex("dbo.Images", "Item_Id");
            CreateIndex("dbo.Types", "Image_Id");
            AddForeignKey("dbo.Images", "Item_Id", "dbo.Items", "Id");
            AddForeignKey("dbo.Types", "Image_Id", "dbo.Images", "Id");
            DropTable("dbo.ImageItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ImageItems",
                c => new
                    {
                        Image_Id = c.Guid(nullable: false),
                        Item_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Image_Id, t.Item_Id });
            
            DropForeignKey("dbo.Types", "Image_Id", "dbo.Images");
            DropForeignKey("dbo.Images", "Item_Id", "dbo.Items");
            DropIndex("dbo.Types", new[] { "Image_Id" });
            DropIndex("dbo.Images", new[] { "Item_Id" });
            DropColumn("dbo.Types", "Image_Id");
            DropColumn("dbo.Images", "Item_Id");
            CreateIndex("dbo.ImageItems", "Item_Id");
            CreateIndex("dbo.ImageItems", "Image_Id");
            AddForeignKey("dbo.ImageItems", "Item_Id", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ImageItems", "Image_Id", "dbo.Images", "Id", cascadeDelete: true);
        }
    }
}
