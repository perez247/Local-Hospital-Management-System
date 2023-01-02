using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class FinancialRequest : BaseEntity
    {
        public decimal? Amount { get; set; }
        public AppCostType AppCostType { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.pending;
        public string? Description { get; set; }
        public virtual FinancialRecord? FinancialRecord { get; set; }
        public Guid? FinancialRecordId { get; set; }
    }
}
