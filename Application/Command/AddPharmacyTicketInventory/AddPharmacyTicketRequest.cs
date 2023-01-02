using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddPharmacyTicketInventory
{
    public class AddPharmacyTicketRequest
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public string? PrescribedPharmacyDosage { get; set; }
        public string? PrescribedQuantity { get; set; }
    }
}
