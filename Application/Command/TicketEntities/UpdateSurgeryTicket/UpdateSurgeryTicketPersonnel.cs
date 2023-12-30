using Application.Annotations;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.UpdateSurgeryTicket
{
    public class UpdateSurgeryTicketPersonnel
    {

        [VerifyGuidAnnotation]
        public string? PersonnelId { get; set; }
        public string? SurgeryRole { get; set; }
        public bool? IsHeadPersonnel { get; set; }
    }
}
