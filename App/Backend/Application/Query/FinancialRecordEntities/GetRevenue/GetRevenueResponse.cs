using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.FinancialRecordEntities.GetRevenue
{
    public class GetRevenueResponse
    {
        public decimal? Profit { get; set; }
        public decimal? Expense { get; set; }
        public decimal? TotalProfit { get; set; }
        public decimal? TotalExpense { get; set; }
    }
}
