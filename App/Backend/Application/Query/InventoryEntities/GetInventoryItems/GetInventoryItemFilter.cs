using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetInventoryItems
{
    public class GetInventoryItemFilter
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }

        [VerifyGuidAnnotation]
        public string? AppInventoryId { get; set; }
        public int? Amount { get; set; }
        public string? AppInventoryType { get; set; }
        public string? CompanyName { get; set; }
        public string? AppInventoryName { get; set; }
        //public List<string>? InventoryItemNames { get; set; }
    }
}
