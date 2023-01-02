using Application.Annotations;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdatePharmacyTicketInventory
{
    public class UpdatePharmacyTicketInventoryRequest
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public int? AppInventoryQuantity { get; set; }
        public decimal? CurrentPrice { get; set; }
        public DateTime? ConcludedDate { get; set; }
        public string? AppTicketStatus { get; set; }
        public string? StaffObservation { get; set; }
        public string? Description { get; set; }
        public ICollection<string>? Proof { get; set; }
    }
}
