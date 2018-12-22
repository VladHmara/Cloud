using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudClient.Model
{
    public class FileBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Directory ParentDirectory { get; set; }
        public string GetUrl()
        {
            if (ParentDirectory != null)
                return ParentDirectory.GetUrl() + "/" + Name;
            else
                return Name;
        }
    }
}
