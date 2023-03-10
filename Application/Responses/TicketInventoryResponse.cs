using Application.Query.GetInventories;
using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class TicketInventoryResponse
    {
        public string? AppTicketId { get; set; }
        public InventoryResponse? Inventory { get; set; }
        public int? AppInventoryQuantity { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? ConcludedDate { get; set; }
        public string? AppTicketStatus { get; set; }
        public ICollection<string> Proof { get; set; } = new List<string>();
        //public virtual Staff? Staff { get; set; }
        //public Guid? StaffId { get; set; }
        //public string? StaffObservation { get; set; }
        public string? Description { get; set; }

        #region Pharmacy section
        public string? PrescribedPharmacyDosage { get; set; }
        public string? PrescribedQuantity { get; set; }
        #endregion

        #region Surgery section
        public DateTime? SurgeryDate { get; set; }
        public string SurgeryTicketStatus { get; set; }
        public IEnumerable<SurgeryTicketPersonnelResponse> SurgeryTicketPersonnels { get; set; }
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

        public static TicketInventoryResponse? Create(TicketInventory ticketInventory)
        {
            if (ticketInventory == null)
            {
                return null;
            }

            return new TicketInventoryResponse
            {
                AppTicketId = ticketInventory.AppTicketId.ToString(),
                Inventory = InventoryResponse.Create(ticketInventory.AppInventory),
                AppInventoryQuantity = ticketInventory.AppInventoryQuantity,
                CurrentPrice = ticketInventory.CurrentPrice,
                TotalPrice = ticketInventory.TotalPrice,
                ConcludedDate = ticketInventory.ConcludedDate,
                AppTicketStatus = ticketInventory.AppTicketStatus.ToString(),
                Proof = ticketInventory.Proof,


                Description = ticketInventory.Description,

                #region Pharmacy section

                PrescribedPharmacyDosage = ticketInventory.PrescribedPharmacyDosage,
                PrescribedQuantity = ticketInventory.PrescribedQuantity,

                #endregion

                #region Surgery section

                SurgeryDate = ticketInventory.SurgeryDate,
                SurgeryTicketStatus = ticketInventory.SurgeryTicketStatus.ToString(),
                SurgeryTicketPersonnels = ticketInventory.SurgeryTicketPersonnels != null && ticketInventory.SurgeryTicketPersonnels.Count() > 0 ? ticketInventory.SurgeryTicketPersonnels.Select(x => SurgeryTicketPersonnelResponse.Create(x)) : null,
                PrescribedSurgeryDescription = ticketInventory.PrescribedSurgeryDescription,

                #endregion

                #region Lab section

                PrescribedLabRadiologyFeature = ticketInventory.PrescribedLabRadiologyFeature,
                DateOfLabTest = ticketInventory.DateOfLabTest,
                LabRadiologyTestResult = ticketInventory.LabRadiologyTestResult,

                #endregion

                #region Admission section

                PrescribedAdmission = ticketInventory.PrescribedAdmission,
                AdmissionEndDate = ticketInventory.AdmissionEndDate,
                AdmissionStartDate = ticketInventory.AdmissionStartDate,

                #endregion

            };
        }
    }

    public class SurgeryTicketPersonnelResponse
    {
        public BaseResponse? Base { get; set; }
        public UserOnlyResponse? Personnel { get; set; }
        public string? SurgeryRole { get; set; }
        public string? Description { get; set; }
        public string? SummaryOfSurgery { get; set; }
        public bool? IsHeadPersonnel { get; set; }
        public bool? IsPatient { get; set; }

        public static SurgeryTicketPersonnelResponse? Create(SurgeryTicketPersonnel surgeryTicketPersonnel)
        {
            if (surgeryTicketPersonnel == null)
            {
                return null;
            }

            return new SurgeryTicketPersonnelResponse
            {
                Base = BaseResponse.Create(surgeryTicketPersonnel),
                Personnel = UserOnlyResponse.Create(surgeryTicketPersonnel.Personnel),
                SurgeryRole = surgeryTicketPersonnel.SurgeryRole,
                Description = surgeryTicketPersonnel.Description,
                SummaryOfSurgery = surgeryTicketPersonnel.SummaryOfSurgery,
                IsHeadPersonnel = surgeryTicketPersonnel.IsHeadPersonnel,
                IsPatient = surgeryTicketPersonnel.IsPatient,
            };
        }
    }
}
