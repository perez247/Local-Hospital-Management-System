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
    }
}
