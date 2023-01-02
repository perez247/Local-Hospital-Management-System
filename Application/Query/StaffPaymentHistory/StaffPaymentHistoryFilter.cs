using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.StaffPaymentHistory
{
    public class StaffPaymentHistoryFilter
    {
        public DateTime? Date { get; set; }
        public bool? Paid { get; set; }
    }
}
