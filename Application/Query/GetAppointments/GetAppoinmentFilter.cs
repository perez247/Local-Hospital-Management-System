using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetAppointments
{
    public class GetAppoinmentFilter
    {
        public DateTime? ExactDate { get; set; }
        public DateTime? StartDate { get; set; }

        [VerifyGuidAnnotation]
        public string? AppointmentId { get; set; }
    }
}
