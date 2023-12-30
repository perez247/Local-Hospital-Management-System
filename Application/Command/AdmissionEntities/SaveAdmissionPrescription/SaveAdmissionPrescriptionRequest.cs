using Application.Annotations;
using Application.Command.TicketEntities.SaveTicketAndInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AdmissionEntities.SaveAdmissionPrescription
{
    public class SaveAdmissionPrescriptionRequest
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }

        [VerifyGuidAnnotation]
        public string? TicketInventoryId { get; set; }
        public string? DoctorsPrescription { get; set; }
        public string? PrescribedQuantity { get; set; }
        public string? AppInventoryType { get; set; }
        public int? Times { get; set; }
        public int? Dosage { get; set; }
        public string? Frequency { get; set; }
        public int? Duration { get; set; }
    }
}
