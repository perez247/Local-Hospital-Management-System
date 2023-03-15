using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddLabTicketInventory
{
    public class AddLabTicketInventoryRequest
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public string? DoctorsPrescription { get; set; }
        public decimal? CurrentPrice { get; set; }
    }
}
