namespace CloudApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v6 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FileBases", name: "Directory_Id", newName: "ParentDirectoryId");
            RenameIndex(table: "dbo.FileBases", name: "IX_Directory_Id", newName: "IX_ParentDirectoryId");
            CreateTable(
                "dbo.UserFileBases",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        FileBase_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.FileBase_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.FileBases", t => t.FileBase_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.FileBase_Id);
            
            AddColumn("dbo.FileBases", "OwnerId", c => c.Guid());
            AddColumn("dbo.FileBases", "Owner_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.FileBases", "Owner_Id");
            AddForeignKey("dbo.FileBases", "Owner_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.FileBases", "DataUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FileBases", "DataUrl", c => c.String());
            DropForeignKey("dbo.FileBases", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserFileBases", "FileBase_Id", "dbo.FileBases");
            DropForeignKey("dbo.UserFileBases", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserFileBases", new[] { "FileBase_Id" });
            DropIndex("dbo.UserFileBases", new[] { "User_Id" });
            DropIndex("dbo.FileBases", new[] { "Owner_Id" });
            DropColumn("dbo.FileBases", "Owner_Id");
            DropColumn("dbo.FileBases", "OwnerId");
            DropTable("dbo.UserFileBases");
            RenameIndex(table: "dbo.FileBases", name: "IX_ParentDirectoryId", newName: "IX_Directory_Id");
            RenameColumn(table: "dbo.FileBases", name: "ParentDirectoryId", newName: "Directory_Id");
        }
    }
}
