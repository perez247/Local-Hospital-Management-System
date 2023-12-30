using Application.Query.AppointmentEntities.GetAppointments;
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
        public AppointmentOnlyResponse? Appointment { get; set; }
        public IEnumerable<TicketInventoryResponse> TicketInventories { get; set; }
        public IEnumerable<FinancailRecordPayerPayeeResponse> PayerPayee { get; set; }

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
                Appointment = AppointmentOnlyResponse.Create(appTicket.Appointment),
                PayerPayee =
                            (appTicket.AppCost != null) &&
                            (appTicket.AppCost.FinancialRecordPayerPayees != null && appTicket.AppCost.FinancialRecordPayerPayees.Count > 0) ?
                            appTicket.AppCost.FinancialRecordPayerPayees.Select(x => FinancailRecordPayerPayeeResponse.Create(x)) :
                            null
            };
        }
    }

    public class AppTicketResponseOnly
    {
        public BaseResponse? Base { get; set; }
        public string? AppointmentId { get; set; }
        public string? OverallDescription { get; set; }
        public bool? Sent { get; set; }
        public bool? SentToFinance { get; set; }
        public string? AppTicketStatus { get; set; }
        public string? AppInventoryType { get; set; }
        public IEnumerable<TicketInventoryResponse> TicketInventories { get; set; }
        public PatientOnlyResponse? Patient { get; set; }
        public static AppTicketResponseOnly? Create(AppTicket appTicket)
        {
            if (appTicket == null)
            {
                return null;
            }

            return new AppTicketResponseOnly
            {
                Base = BaseResponse.Create(appTicket),
                AppointmentId = appTicket.AppointmentId?.ToString(),
                OverallDescription = appTicket.OverallDescription,
                Sent = appTicket.Sent,
                SentToFinance = appTicket.SentToFinance,
                AppInventoryType = appTicket.AppInventoryType.ToString(),
                AppTicketStatus = appTicket.AppTicketStatus.ToString(),
                TicketInventories = appTicket.TicketInventories != null && appTicket.TicketInventories.Count() > 0 ? appTicket.TicketInventories.Select(x => TicketInventoryResponse.Create(x)) : null,
                Patient = PatientOnlyResponse.Create(appTicket.Appointment.Patient),
            };
        }
    }
}
