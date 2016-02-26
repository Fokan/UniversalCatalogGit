namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr36 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageItems",
                c => new
                {
                    Image_Id = c.Guid(nullable: false),
                    Item_Id = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.Image_Id, t.Item_Id });

            //DropForeignKey("dbo.Types", "Image_Id", "dbo.Images");
            DropForeignKey("dbo.Images", "Item_Id", "dbo.Items");
            CreateIndex("dbo.ImageItems", "Item_Id");
            CreateIndex("dbo.ImageItems", "Image_Id");
            AddForeignKey("dbo.ImageItems", "Item_Id", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ImageItems", "Image_Id", "dbo.Images", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
        }
    }
}
