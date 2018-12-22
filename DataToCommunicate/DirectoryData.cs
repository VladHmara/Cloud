using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataToCommunicate
{
    public class DirectoryData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentDirectoryId { get; set; }
        public int FilesCount { get; set; }
    }
}
