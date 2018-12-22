namespace CloudApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FileBases", new[] { "ParentDirectoryId" });
            AlterColumn("dbo.FileBases", "ParentDirectoryId", c => c.Guid());
            CreateIndex("dbo.FileBases", "ParentDirectoryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.FileBases", new[] { "ParentDirectoryId" });
            AlterColumn("dbo.FileBases", "ParentDirectoryId", c => c.Guid(nullable: false));
            CreateIndex("dbo.FileBases", "ParentDirectoryId");
        }
    }
}
