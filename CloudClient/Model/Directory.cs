using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudClient.Model
{
    public class Directory : FileBase
    {
        public List<FileBase> Files { get; set; } = new List<FileBase>();
        public int FilesCount { get; set; }
    }
}
