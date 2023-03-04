using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetTickets
{
    public class GetTicketsQueryFilter
    {
        [VerifyGuidAnnotation]
        public string? AppointmentId { get; set; }

        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }

        [VerifyGuidAnnotation]
        public string? DoctorId { get; set; }
        public string? AppInventoryType { get; set; }
    }
}
