using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetInventories
{
    public class GetInventoriesFilter
    {
        public string? Name { get; set; }
        public string? AppInventoryType { get; set; }
        public string? Quantity { get; set; }

        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
    }
}
