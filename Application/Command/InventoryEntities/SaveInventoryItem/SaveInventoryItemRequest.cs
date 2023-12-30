using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.SaveInventoryItem
{
    public class SaveInventoryItemRequest
    {

        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }
        public decimal? CompanyAmount { get; set; }
        public int Index { get; set; }
    }
}
