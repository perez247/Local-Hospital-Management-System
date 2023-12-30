using Application.Requests;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.ConcludeSurgeryTicket
{
    public class ConcludeSurgeryTicketRequest : ConcludeTicketRequest
    {
        public string? SurgeryTestResult { get; set; }
        public ICollection<TicketInventoryItemUsed>? ItemsUsed { get; set; }
    }
}
