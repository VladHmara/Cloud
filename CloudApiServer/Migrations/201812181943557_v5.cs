namespace CloudApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FileBases", "Data_Id", "dbo.FileDatas");
            DropIndex("dbo.FileBases", new[] { "Data_Id" });
            AddColumn("dbo.FileBases", "DataUrl", c => c.String());
            DropColumn("dbo.FileBases", "Data_Id");
            DropTable("dbo.FileDatas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FileDatas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.FileBases", "Data_Id", c => c.Guid());
            DropColumn("dbo.FileBases", "DataUrl");
            CreateIndex("dbo.FileBases", "Data_Id");
            AddForeignKey("dbo.FileBases", "Data_Id", "dbo.FileDatas", "Id");
        }
    }
}
