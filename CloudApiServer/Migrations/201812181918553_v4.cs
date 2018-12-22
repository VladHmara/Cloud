namespace CloudApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserFileBases", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserFileBases", "FileBase_Id", "dbo.FileBases");
            DropForeignKey("dbo.FileBases", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.FileBases", new[] { "Owner_Id" });
            DropIndex("dbo.UserFileBases", new[] { "User_Id" });
            DropIndex("dbo.UserFileBases", new[] { "FileBase_Id" });
            RenameColumn(table: "dbo.FileBases", name: "ParentDirectoryId", newName: "Directory_Id");
            RenameIndex(table: "dbo.FileBases", name: "IX_ParentDirectoryId", newName: "IX_Directory_Id");
            DropColumn("dbo.FileBases", "OwnerId");
            DropColumn("dbo.FileBases", "Owner_Id");
            DropTable("dbo.UserFileBases");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserFileBases",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        FileBase_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.FileBase_Id });
            
            AddColumn("dbo.FileBases", "Owner_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.FileBases", "OwnerId", c => c.Guid());
            RenameIndex(table: "dbo.FileBases", name: "IX_Directory_Id", newName: "IX_ParentDirectoryId");
            RenameColumn(table: "dbo.FileBases", name: "Directory_Id", newName: "ParentDirectoryId");
            CreateIndex("dbo.UserFileBases", "FileBase_Id");
            CreateIndex("dbo.UserFileBases", "User_Id");
            CreateIndex("dbo.FileBases", "Owner_Id");
            AddForeignKey("dbo.FileBases", "Owner_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserFileBases", "FileBase_Id", "dbo.FileBases", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserFileBases", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
