using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.ConcludeSurgeryTicketInventory
{
    public class ConcludeSurgeryTicketInventoryRequest
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public string? SurgeryTicketStatus { get; set; }
        public DateTime? ConcludedDate { get; set; }
        public string? AppTicketStatus { get; set; }
        public ICollection<string>? Proof { get; set; }
    }
}
