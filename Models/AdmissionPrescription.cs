using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AdmissionPrescription : BaseEntity
    {
        public virtual AppTicket? AppTicket { get; set; }
        public Guid? AppTicketId { get; set; }
        public string? OverallDescription { get; set; }
        public AppTicketStatus AppTicketStatus { get; set; } = AppTicketStatus.ongoing;
        public AppInventoryType AppInventoryType { get; set; }
        public ICollection<TicketInventory> TicketInventories { get; set; } = new List<TicketInventory>();
    }
}
