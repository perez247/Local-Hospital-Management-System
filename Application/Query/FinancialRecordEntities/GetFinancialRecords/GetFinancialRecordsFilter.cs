using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.FinancialRecordEntities.GetFinancialRecords
{
    public class GetFinancialRecordsFilter
    {
        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }

        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
