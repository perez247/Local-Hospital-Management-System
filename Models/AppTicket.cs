using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppTicket : BaseEntity
    {
        public virtual AppAppointment? Appointment { get; set; }
        public Guid? AppointmentId { get; set; }
        public virtual AppCost? AppCost { get; set; }
        public Guid? AppCostId { get; set; }
        public string? OverallDescription { get; set; }
        public bool? Sent { get; set; }
        public bool? SentToFinance { get; set; }
        public AppTicketStatus AppTicketStatus { get; set; } = AppTicketStatus.ongoing;
        public AppInventoryType AppInventoryType { get; set; }
        public ICollection<TicketInventory> TicketInventories { get; set; } = new List<TicketInventory>();
    }
}


