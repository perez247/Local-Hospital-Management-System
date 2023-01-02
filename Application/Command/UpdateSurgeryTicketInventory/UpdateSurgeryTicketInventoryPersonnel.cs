using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateSurgeryTicketInventory
{
    public class UpdateSurgeryTicketInventoryPersonnel
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }
        public string? SurgeryRole { get; set; }
        public string? Description { get; set; }
        public bool IsHeadPersonnel { get; set; }
        public bool IsPatient { get; set; }
    }
}
