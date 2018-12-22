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
    public class Directory : FileBase
    {
        public virtual ICollection<FileBase> Files { get; set; } = new List<FileBase>();
    }
}
