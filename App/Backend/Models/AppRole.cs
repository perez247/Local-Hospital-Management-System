using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppRole : IdentityRole<Guid>
    {
        /// <summary>
        /// List of roles of the application
        /// </summary>
        /// <value></value>
        public ICollection<AppUserRole> UserRoles { get; private set; } = new List<AppUserRole>();
    }
}
