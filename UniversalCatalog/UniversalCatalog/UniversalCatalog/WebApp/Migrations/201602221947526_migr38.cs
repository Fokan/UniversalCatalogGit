namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr38 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "Item_Id", "dbo.Items");
            DropIndex("dbo.Images", new[] { "Item_Id" });
            CreateTable(
                "dbo.ImageItems",
                c => new
                    {
                        Image_Id = c.Guid(nullable: false),
                        Item_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Image_Id, t.Item_Id })
                .ForeignKey("dbo.Images", t => t.Image_Id, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.Item_Id, cascadeDelete: true)
                .Index(t => t.Image_Id)
                .Index(t => t.Item_Id);
            
            //DropColumn("dbo.Images", "Item_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "Item_Id", c => c.Guid());
            DropForeignKey("dbo.ImageItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.ImageItems", "Image_Id", "dbo.Images");
            DropIndex("dbo.ImageItems", new[] { "Item_Id" });
            DropIndex("dbo.ImageItems", new[] { "Image_Id" });
            DropTable("dbo.ImageItems");
            CreateIndex("dbo.Images", "Item_Id");
            AddForeignKey("dbo.Images", "Item_Id", "dbo.Items", "Id");
        }
    }
}
