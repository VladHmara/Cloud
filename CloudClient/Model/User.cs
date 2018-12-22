using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CloudClient.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Image Image { get; set; }
    }
}
