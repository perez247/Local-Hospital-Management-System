using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TicketInventoryDebtor : BaseEntity
    {
        public Guid? PayerId { get; set; }
        public AppUser? Payer { get; set; }
        public Guid? TicketInventoryId { get; set; }
        public TicketInventory? TicketInventory { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
    }
}
