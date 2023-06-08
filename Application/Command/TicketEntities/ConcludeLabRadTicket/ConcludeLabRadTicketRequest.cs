using Application.Requests;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.ConcludeLabRadTicket
{
    public class ConcludeLabRadTicketRequest : ConcludeTicketRequest
    {
        public string? LabRadiologyTestResult { get; set; }
        public ICollection<TicketInventoryItemUsed>? ItemsUsed { get; set; }
    }
}
