using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.AppointmentEntities.GetAppointmentCountInAMonth
{
    public class AppointmentDayCountResponse
    {
        public int Total { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}
