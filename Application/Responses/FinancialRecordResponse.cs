using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class FinancialRecordResponse
    {
        public decimal? Amount { get; set; }
        public decimal? ApprovedPrice { get; set; }
        public string? Description { get; set; }
        public IEnumerable<PaymentResponse>? Payment { get; set; }
        public string? CostType { get; set; }
        public string? PaymentStatus { get; set; }
        public BaseResponse? Base { get; set; }
        public IEnumerable<FinancailRecordPayerPayeeResponse> PayerPayee { get; set; }
        public int TotalAppCosts { get; set; }
        public UserOnlyResponse? Actor { get; set; }

        public static FinancialRecordResponse? Create(FinancialRecord financialRecord)
        {
            if (financialRecord == null)
            {
                return null;
            }

            return new FinancialRecordResponse
            {
                Amount = financialRecord.Amount,
                ApprovedPrice = financialRecord.ApprovedAmount,
                Description = financialRecord.Description,
                Payment = financialRecord.Payments.Select(x => PaymentResponse.Create(x)),
                CostType = financialRecord.CostType.ToString(),
                PaymentStatus = financialRecord.PaymentStatus.ToString(),
                Base = BaseResponse.Create(financialRecord),
                PayerPayee =
                            (financialRecord.FinancialRecordPayerPayees != null && financialRecord.FinancialRecordPayerPayees.Count > 0) ?
                            financialRecord.FinancialRecordPayerPayees.Select(x => FinancailRecordPayerPayeeResponse.Create(x)) :
                            null,
                TotalAppCosts = financialRecord.TotalAppCosts,
                Actor = UserOnlyResponse.Create(financialRecord.Actor)
            };
        }
    }
}
