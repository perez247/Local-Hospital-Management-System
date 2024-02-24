using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.SaveTicketInventory
{
    public class SaveTicketInventoryDebtor
    {
        [VerifyGuidAnnotation]
        public string? PayerId { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
    }
}
