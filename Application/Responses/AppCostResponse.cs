using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class AppCostResponse
    {
        public decimal? Amount { get; set; }
        public decimal? ApprovedPrice { get; set; }
        public string? Description { get; set; }
        public IEnumerable<PaymentResponse>? Payment { get; set; }
        public string? CostType { get; set; }
        public string? PaymentStatus { get; set; }
        public AppTicketResponseOnly? Ticket { get; set; }
        public AppTicketResponseOnly? TicketPart { get; set; }
        public bool PatientContract { get; set; }
        public bool CompanyContract { get; set; }
        public IEnumerable<FinancailRecordPayerPayeeResponse> PayerPayee { get; set; }

        public BaseResponse? Base { get; set; }

        public static AppCostResponse? Create(AppCost appCost)
        {
            if (appCost == null)
            {
                return null;
            }

            return new AppCostResponse
            {
                Amount = appCost.Amount,
                ApprovedPrice = appCost.ApprovedPrice,
                Description = appCost.Description,
                Payment = appCost.Payments.Select(x => PaymentResponse.Create(x)),
                CostType = appCost.CostType.ToString(),
                PaymentStatus = appCost.PaymentStatus.ToString(),
                Base = BaseResponse.Create(appCost),
                PayerPayee =
                            (appCost.FinancialRecordPayerPayees != null && appCost.FinancialRecordPayerPayees.Count > 0) ?
                            appCost.FinancialRecordPayerPayees.Select(x => FinancailRecordPayerPayeeResponse.Create(x)) :
                            null,
                Ticket = appCost.AppTicket != null ? AppTicketResponseOnly.Create(appCost.AppTicket) : null,
                TicketPart = appCost.AppTicketPart != null ? AppTicketResponseOnly.Create(appCost.AppTicketPart) : null,
                PatientContract = appCost.PatientContract != null,
                CompanyContract = appCost.CompanyContract != null,
            };
        }
    }
    public class PaymentResponse
    {
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public string? PaymentType { get; set; }
        public string? Proof { get; set; }
        public string? PaymentDetails { get; set; }
        public string? AdditionalDetail { get; set; }
        public DateTime? DatePaid { get; set; }

        public static PaymentResponse? Create(Payment payment)
        {
            if (payment == null)
            {
                return null;
            }

            return new PaymentResponse
            {
                Amount = payment.Amount,
                Tax = payment.Tax,
                PaymentType = payment.PaymentType.ToString(),
                Proof = payment.Proof,
                PaymentDetails = payment.PaymentDetails,
                AdditionalDetail = payment.AdditionalDetail,
                DatePaid = payment.DatePaid,
            };
        }
    }
}
