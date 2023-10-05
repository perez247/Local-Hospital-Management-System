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
        public StaffResponse? Staff { get; set; }
        //public Guid? StaffId { get; set; }
        //public string? StaffObservation { get; set; }
        public string? DoctorsPrescription { get; set; }
        public string? Description { get; set; }
        public string? DepartmentDescription { get; set; }
        public string? FinanceDescription { get; set; }
        public IEnumerable<TicketInventoryItemUsedResponse>? ItemsUsed { get; set; } = new List<TicketInventoryItemUsedResponse>();
        public int? Times { get; set; }
        public int? Dosage { get; set; }
        public string? Frequency { get; set; }
        public AdmissionPrescriptionResponse? AdmissionPrescription { get; set; }

        #region Pharmacy section
        public string? PrescribedQuantity { get; set; }
        #endregion

        #region Surgery section
        public DateTime? SurgeryDate { get; set; }
        public string? SurgeryTestResult { get; set; }
        public string? SurgeryTicketStatus { get; set; }
        public IEnumerable<SurgeryTicketPersonnelResponse>? SurgeryTicketPersonnels { get; set; }
        #endregion

        #region Lab section
        public DateTime? DateOfLabTest { get; set; }
        public string? LabRadiologyTestResult { get; set; }
        #endregion

        #region Admission section
        public string? PrescribedAdmission { get; set; }
        public DateTime? AdmissionStartDate { get; set; }
        public DateTime? AdmissionEndDate { get; set; }
        #endregion

        public BaseResponse? Base { get; set; }

        public static TicketInventoryResponse? Create(TicketInventory ticketInventory, bool includePrescription = false)
        {
            if (ticketInventory == null)
            {
                return null;
            }

            var data = new TicketInventoryResponse
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
                DoctorsPrescription = ticketInventory.DoctorsPrescription,
                DepartmentDescription = ticketInventory.DepartmentDescription,
                FinanceDescription = ticketInventory.FinanceDescription,

                ItemsUsed = ticketInventory.ItemsUsed != null && ticketInventory.ItemsUsed.Count > 0 ? ticketInventory.ItemsUsed.Select(x => TicketInventoryItemUsedResponse.Create(x)) : null,

                Times = ticketInventory.Times,
                Dosage = ticketInventory.Dosage,
                Frequency = ticketInventory.Frequency,

                Staff = StaffResponse.Create(ticketInventory.Staff),

                #region Pharmacy section
                PrescribedQuantity = ticketInventory.PrescribedQuantity,

                #endregion

                #region Surgery section

                SurgeryDate = ticketInventory.SurgeryDate,
                SurgeryTestResult = ticketInventory.SurgeryTestResult,
                SurgeryTicketStatus = ticketInventory.SurgeryTicketStatus.ToString(),
                SurgeryTicketPersonnels = ticketInventory.SurgeryTicketPersonnels != null && ticketInventory.SurgeryTicketPersonnels.Count() > 0 ? ticketInventory.SurgeryTicketPersonnels.Select(x => SurgeryTicketPersonnelResponse.Create(x)) : null,

                #endregion

                #region Lab section

                DateOfLabTest = ticketInventory.DateOfLabTest,
                LabRadiologyTestResult = ticketInventory.LabRadiologyTestResult,

                #endregion

                #region Admission section

                AdmissionEndDate = ticketInventory.AdmissionEndDate,
                AdmissionStartDate = ticketInventory.AdmissionStartDate,

                #endregion

                Base = BaseResponse.Create(ticketInventory),

            };

            if (includePrescription)
            {
                data.AdmissionPrescription = AdmissionPrescriptionResponse.Create(ticketInventory.AdmissionPrescription);
            }

            return data;
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

    public class TicketInventoryItemUsedResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }

        public static TicketInventoryItemUsedResponse? Create(TicketInventoryItemUsed ticketInventoryItemUsed)
        {
            if (ticketInventoryItemUsed == null)
            {
                return null;
            }

            return new TicketInventoryItemUsedResponse
            {
                Id = ticketInventoryItemUsed.Id,
                Name = ticketInventoryItemUsed.Name,
                Quantity = ticketInventoryItemUsed.Quantity,
            };
        }
    }
}
