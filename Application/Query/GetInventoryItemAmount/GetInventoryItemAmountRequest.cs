using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetInventoryItemAmount
{
    public class GetInventoryItemAmountRequest
    {
        [VerifyGuidAnnotation]
        public string? AppInventoryId { get; set; }
    }
}
