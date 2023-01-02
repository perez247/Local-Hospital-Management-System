using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class IdentityBaseUser : IdentityUser<Guid>
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
