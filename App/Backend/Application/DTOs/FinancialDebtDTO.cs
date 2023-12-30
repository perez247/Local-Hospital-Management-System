using Application.Responses;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FinancialDebtDTO
    {
        public ICollection<AppCost>? AppCosts { get; set; }
        public IEnumerable<AppCostResponse>? Result { get; set; }
        public decimal Debt { get; set; }
        public decimal Paid { get; set; }
    }
}
