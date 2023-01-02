using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateLabTicketInventory
{
    public class UpdateLabTicketInventoryRequest
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public DateTime? DateOfLabTest { get; set; }
    }
}
