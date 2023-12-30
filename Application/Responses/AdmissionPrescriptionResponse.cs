using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class AdmissionPrescriptionResponse
    {
        public BaseResponse? Base { get; set; }
        public string? AppTicketId { get; set; }
        public string? OverallDescription { get; set; }
        public string? AppTicketStatus { get; set; }
        public string? AppInventoryType { get; set; }
        public StaffResponse Doctor { get; set; }
        public AppTicketResponse? Ticket { get; set; }
        public IEnumerable<TicketInventoryResponse> TicketInventories { get; set; } = new List<TicketInventoryResponse>();
    
        public static AdmissionPrescriptionResponse? Create(AdmissionPrescription admissionPrescription)
        {
            if (admissionPrescription == null)
            {
                return null;
            }

            var staff = admissionPrescription.Doctor.Staff;
            staff.AppUser = admissionPrescription.Doctor;

            return new AdmissionPrescriptionResponse
            {
                Base = BaseResponse.Create(admissionPrescription),
                AppTicketId = admissionPrescription.AppTicketId.ToString(),
                OverallDescription = admissionPrescription.OverallDescription,
                AppTicketStatus = admissionPrescription.AppTicketStatus.ToString(),
                AppInventoryType = admissionPrescription.AppInventoryType.ToString(),
                Doctor = StaffResponse.Create(staff),
                TicketInventories = admissionPrescription.TicketInventories != null && admissionPrescription.TicketInventories.Count > 0 ?
                                    admissionPrescription.TicketInventories.Select(x => TicketInventoryResponse.Create(x)) : null,
            };
        }
    }
}
