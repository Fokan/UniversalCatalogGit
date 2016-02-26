namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Types", new[] { "ParentType_Id" });
            RenameColumn(table: "dbo.Types", name: "ParentType_Id", newName: "ParentTypeId");
            AlterColumn("dbo.Types", "ParentTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Types", "ParentTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Types", new[] { "ParentTypeId" });
            AlterColumn("dbo.Types", "ParentTypeId", c => c.Guid());
            RenameColumn(table: "dbo.Types", name: "ParentTypeId", newName: "ParentType_Id");
            CreateIndex("dbo.Types", "ParentType_Id");
        }
    }
}
