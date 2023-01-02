using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppCost : BaseEntity
    {
        public virtual Staff? FinancialApprover { get; set; }
        public Guid? FinancialApproverId { get; set; }
        public virtual FinancialRecord? FinancialRecord { get; set; }
        public Guid? FinancialRecordId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? ApprovedPrice { get; set; }
        public string? Description { get; set; }
        public ICollection<Payment>? Payments { get; set; } = new List<Payment>();
        public AppCostType CostType { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.pending;
        public PatientContract? PatientContract { get; set; }
        public CompanyContract? CompanyContract { get; set; }
        public AppTicket? AppTicket { get; set; }
    }

    public class Payment
    {
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public PaymentType PaymentType { get; set; } = PaymentType.none;
        public string? Proof { get; set; }
        public string? PaymentDetails { get; set; }
        public string? AdditionalDetail { get; set; }
    }
}
