using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ActivityLog : BaseEntity
    {
        public Guid? ActorId { get; set; }
        public AppUser? Actor { get; set; }
        public Guid? ActeeId { get; set; }
        public AppUser? Actee { get; set; }
        public string? ObjectType { get; set; }
        public string? ObjectId { get; set; }
        public string? ActionType { get; set;}
        public string? ActionDescription { get; set; }
        public string? OtherDescription { get; set; }
    }
}
