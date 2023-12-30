using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.FinancialRecordEntities.GetPendingUserContracts
{
    public class GetPendingUserContractsFilter
    {
        public string? PaymentStatus { get; set; }
        public bool Patient { get; set; }
        public bool Company { get; set; }
    }
}
