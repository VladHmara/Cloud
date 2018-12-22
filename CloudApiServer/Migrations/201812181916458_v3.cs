namespace CloudApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FileBases", "OwnerId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FileBases", "OwnerId", c => c.Guid(nullable: false));
        }
    }
}
