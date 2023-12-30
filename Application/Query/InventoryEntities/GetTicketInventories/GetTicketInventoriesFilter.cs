using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetTicketInventories
{
    public class GetTicketInventoriesFilter
    {
        [VerifyGuidAnnotation]
        public string? AppticketId { get; set; }

        [VerifyGuidAnnotation]
        public string? PrescriptionId { get; set; }
        public bool? isTickets { get; set; }
        public bool? isPrescriptions { get; set; }
        public ICollection<string>? roles { get; set; } = new List<string>();
    }
}
