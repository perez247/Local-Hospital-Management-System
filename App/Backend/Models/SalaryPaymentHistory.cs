using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SalaryPaymentHistory : BaseEntity
    {
        public virtual Staff? Staff { get; set; }
        public Guid? StaffId { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal Savings { get; set; }
        public bool Paid { get; set; }
        public DateTime DatePaidFor { get; set; }
    }
}
