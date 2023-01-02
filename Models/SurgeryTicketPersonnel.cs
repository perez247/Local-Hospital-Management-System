using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SurgeryTicketPersonnel : BaseEntity
    {
        public virtual TicketInventory? TicketInventory { get; set; }
        public Guid? TicketInventoryId { get; set; }
        public virtual AppUser? Personnel { get; set; }
        public Guid? PersonnelId { get; set; }
        public string? SurgeryRole { get; set; }
        public string? Description { get; set; }
        public string? SummaryOfSurgery { get; set; }
        public bool? IsHeadPersonnel { get; set; }
        public bool? IsPatient { get; set; }
    }
}
