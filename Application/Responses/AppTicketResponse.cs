using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class AppTicketResponse
    {
        public BaseResponse? Base { get; set; }
        public string? AppointmentId { get; set; }
        public AppCostResponse? Cost { get; set; }
        public string? OverallDescription { get; set; }
        public string? OverallAppointmentDescription { get; set; }
        public bool? Sent { get; set; }
        public bool? SentToFinance { get; set; }
        public string? AppTicketStatus { get; set; }
        public string? AppInventoryType { get; set; }
        public StaffResponse? Doctor { get; set; }
        public PatientResponse? Patient { get; set; }
        public PatientResponse? PatientPaying { get; set; }
        public CompanyResponse? CompanyPaying { get; set; }
        public IEnumerable<TicketInventoryResponse> TicketInventories { get; set; }

        public static AppTicketResponse? Create(AppTicket appTicket)
        {
            if (appTicket == null)
            {
                return null;
            }

            return new AppTicketResponse
            {
                Base = BaseResponse.Create(appTicket),
                AppointmentId = appTicket.AppointmentId?.ToString(),
                Cost = AppCostResponse.Create(appTicket.AppCost),
                OverallDescription = appTicket.OverallDescription,
                Sent = appTicket.Sent,
                SentToFinance = appTicket.SentToFinance,
                AppInventoryType = appTicket.AppInventoryType.ToString(),
                AppTicketStatus = appTicket.AppTicketStatus.ToString(),
                TicketInventories = appTicket.TicketInventories != null && appTicket.TicketInventories.Count() > 0 ? appTicket.TicketInventories.Select(x => TicketInventoryResponse.Create(x)) : null,
                Doctor = StaffResponse.Create(appTicket.Appointment.Doctor),
                Patient = PatientResponse.Create(appTicket.Appointment.Patient),
                OverallAppointmentDescription = appTicket.Appointment.OverallDescription,
                PatientPaying = PatientResponse.Create(appTicket.Patient),
                CompanyPaying = CompanyResponse.Create(appTicket.Company),
            };
        }
    }
}
