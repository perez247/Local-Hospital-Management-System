using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests
{
    public class SendTicketToFinanceRequest
    {
        [VerifyGuidAnnotation]
        public string? TicketInventoryId { get; set; }

        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public string? AppTicketStatus { get; set; }
        public int? PrescribedQuantity { get; set; }
        public string? DepartmentDescription { get; set; }
    }
}
