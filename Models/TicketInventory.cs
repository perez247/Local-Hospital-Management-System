using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TicketInventory : BaseEntity
    {
        public virtual AppTicket? AppTicket { get; set; }
        public Guid? AppTicketId { get; set; }
        public virtual AppInventory? AppInventory { get; set; }
        public Guid? AppInventoryId { get; set; }
        public int? AppInventoryQuantity { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? ConcludedDate { get; set; }
        public AppTicketStatus AppTicketStatus { get; set; } = AppTicketStatus.ongoing;
        public ICollection<string> Proof { get; set; } = new List<string>();
        public virtual Staff? Staff { get; set; }
        public Guid? StaffId { get; set; }
        public string? StaffObservation { get; set; }
        public string? Description { get; set; }

        #region Pharmacy section
        public string? PrescribedPharmacyDosage { get; set; }
        public string? PrescribedQuantity { get; set; }
        #endregion

        #region Surgery section
        public DateTime? SurgeryDate { get; set; }
        public SurgeryTicketStatus SurgeryTicketStatus { get; set; } = SurgeryTicketStatus.unknown;
        public ICollection<SurgeryTicketPersonnel> SurgeryTicketPersonnels { get; set; } = new List<SurgeryTicketPersonnel>();
        public string? PrescribedSurgeryDescription { get; set; }
        #endregion

        #region Lab section
        public string? PrescribedLabRadiologyFeature { get; set; }
        public DateTime? DateOfLabTest { get; set; }
        public string? LabRadiologyTestResult { get; set; }
        #endregion

        #region Admission section
        public string? PrescribedAdmission { get; set; }
        public DateTime? AdmissionStartDate { get; set; }
        public DateTime? AdmissionEndDate { get; set; }
        #endregion
    }
}
