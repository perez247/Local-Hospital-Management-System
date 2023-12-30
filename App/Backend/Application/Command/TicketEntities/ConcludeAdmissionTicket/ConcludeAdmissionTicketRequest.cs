using Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.ConcludeAdmissionTicket
{
    public class ConcludeAdmissionTicketRequest : ConcludeTicketRequest
    {
        public decimal? TotalPrice { get; set; }
        public DateTime? AdmissionEndDate { get; set; }
    }
}
