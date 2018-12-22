using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataToCommunicate
{
    public class CreateDirectoryData
    {
        public string Name { get; set; }
        public Guid? ParentDirectoryId { get; set; }
    }
}
