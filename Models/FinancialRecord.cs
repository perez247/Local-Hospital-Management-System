using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class FinancialRecord : BaseEntity
    {
        public decimal? Amount { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public AppCostType CostType { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.pending;
        public ICollection<Payment>? Payments { get; set; } = new List<Payment>();
        public string? Description { get; set; }
        public ICollection<AppCost> AppCosts { get; set; } = new List<AppCost>();
        public int TotalAppCosts { get; set; }
        public ICollection<FinancialRecordPayerPayee> FinancialRecordPayerPayees { get; set; } = new List<FinancialRecordPayerPayee>();
        public ICollection<FinancialRequest> FinancialRequest { get; set; } = new List<FinancialRequest>();
    }
}
