using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class FinancialRecordPayerPayee : BaseEntity
    {
        public virtual FinancialRecord? FinancialRecord { get; set; }
        public Guid? FinancialRecordId { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public Guid? AppUserId { get; set; }
        public virtual AppCost? AppCost { get; set; }
        public Guid? AppCostId { get; set; }
        public bool Payer { get; set; } = false;
    }
}
