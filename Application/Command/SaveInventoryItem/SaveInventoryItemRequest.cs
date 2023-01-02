using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.SaveInventoryItem
{
    public class SaveInventoryItemRequest
    {
        public string? InventoryId { get; set; }

        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }

        [VerifyGuidAnnotation]
        public string? InventoryItemId { get; set; }

        public decimal? NewPrice { get; set; }
        public int Index { get; set; }
    }
}
