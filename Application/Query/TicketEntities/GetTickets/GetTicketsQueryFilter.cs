using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.TicketEntities.GetTickets
{
    public class GetTicketsQueryFilter
    {
        [VerifyGuidAnnotation]
        public string? AppointmentId { get; set; }

        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }

        [VerifyGuidAnnotation]
        public string? DoctorId { get; set; }

        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
        public string? AppInventoryType { get; set; }
        public bool? Full { get; set; }
        public bool? SentToDepartment { get; set; }
        public bool? SentToFinance { get; set; }
        public string? AppTicketStatus { get; set; }
    }
}
