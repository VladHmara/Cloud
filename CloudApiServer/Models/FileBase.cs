using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CloudApiServer.Models
{
    public abstract class FileBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> UsersWithAccess { get; set; } = new List<User>();
        public User Owner { get; set; }
        public Guid? OwnerId { get; set; }
        public Directory ParentDirectory { get; set; }
        public Guid? ParentDirectoryId { get; set; }
    }
}
