namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr31 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "Type_Id", "dbo.Types");
            DropIndex("dbo.Items", new[] { "Type_Id" });
            RenameColumn(table: "dbo.Items", name: "Type_Id", newName: "TypeId");
            AlterColumn("dbo.Items", "TypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Items", "TypeId");
            AddForeignKey("dbo.Items", "TypeId", "dbo.Types", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "TypeId", "dbo.Types");
            DropIndex("dbo.Items", new[] { "TypeId" });
            AlterColumn("dbo.Items", "TypeId", c => c.Guid());
            RenameColumn(table: "dbo.Items", name: "TypeId", newName: "Type_Id");
            CreateIndex("dbo.Items", "Type_Id");
            AddForeignKey("dbo.Items", "Type_Id", "dbo.Types", "Id");
        }
    }
}
