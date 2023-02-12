using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppFile : BaseEntity
    {
        public string? Name { get; set; }
        public string? Base64String { get; set; }
    }
}
