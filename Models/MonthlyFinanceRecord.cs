using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MonthlyFinanceRecord : BaseEntity
    {
        public decimal Profit { get; set; }
        public decimal Expense { get; set; }
        public DateTime Date { get; set; }
        public DateTime DisplayDate { get; set; }
    }
}
