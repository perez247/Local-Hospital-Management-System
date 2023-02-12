using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserFile : AppFile
    {
        public virtual AppUser? AppUser { get; set; }
        public Guid? AppUserId { get; set; }
    }
}
