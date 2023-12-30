using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        /// <summary>
        /// User
        /// </summary>
        /// <value></value>
        public AppUser User { get; set; } = null!;

        /// <summary>
        /// Role
        /// </summary>
        /// <value></value>
        public AppRole Role { get; set; } = null!;
    }
}
