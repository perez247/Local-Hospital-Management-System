using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateSurgeryTicketInventory
{
    public class UpdateSurgeryTicketInventoryRequest
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public ICollection<UpdateSurgeryTicketInventoryPersonnel>? UpdateSurgeryTicketInventoryPersonnel { get; set; }
    }
}
