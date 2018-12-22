using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApiServer.Models
{
    public class CloudDbContext : IdentityDbContext<User>
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Directory> Directories { get; set; }
        public DbSet<FileBase> FileBases { get; set; }

        public CloudDbContext() : base("CloudDB")
        {
        }
    }
}
