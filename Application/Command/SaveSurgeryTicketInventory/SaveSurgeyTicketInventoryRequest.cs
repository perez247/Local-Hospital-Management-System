using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Annotations;

namespace Application.Command.SaveSurgeryTicketInventory
{
    public class SaveSurgeyTicketInventoryRequest
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public string? DoctorsPrescription { get; set; }
        public decimal? CurrentPrice { get; set; }
    }
}
