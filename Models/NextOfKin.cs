﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class NextOfKin : BaseEntity
    {
        public virtual AppUser? AppUser { get; set; }
        public Guid? AppUserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Profile { get; set; }
    }
}
