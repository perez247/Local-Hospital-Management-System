using Application.Annotations;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.ConcludePharmacyTicket
{
    public class ConcludePharmacyTicketRequest
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public DateTime? ConcludedDate { get; set; }
        public string? AppTicketStatus { get; set; }
        public ICollection<string>? Proof { get; set; }
    }
}
