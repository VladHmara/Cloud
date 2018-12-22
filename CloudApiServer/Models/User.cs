using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CloudApiServer.Models
{
    public class User : IdentityUser
    {
        [InverseProperty("UsersWithAccess")]
        public virtual ICollection<FileBase> AcessFiles { get; set; } = new List<FileBase>();
        [InverseProperty("Owner")]
        public virtual ICollection<FileBase> UserFiles { get; set; } = new List<FileBase>();
    }
}
