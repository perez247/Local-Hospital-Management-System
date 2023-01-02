﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppUser : IdentityBaseUser
    {
        public virtual Staff? Staff { get; set; }
        public virtual NextOfKin? NextOfKin { get; set; }
        public virtual Patient? Patient { get; set; }
        public virtual Company? Company { get; set; }
        public Guid? StaffId { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Address { get; set; }
        public ICollection<SurgeryTicketPersonnel> SurgeryTicketPersonnels { get; set; } = new List<SurgeryTicketPersonnel>();

    }
}