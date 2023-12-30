using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class DashboardStatsResponse
    {
        public int TotalPatients { get; set; }
        public int TotalCompanies { get; set; }
        public int AppointmentsToday { get; set; }
        public int TicketsToday { get; set; }
    }
}
