using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.AdmissionEntities.GetPrescriptions
{
    public class GetPrescriptionQueryFilter
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }

        [VerifyGuidAnnotation]
        public string? PrescriptionId { get; set; }
    }
}
