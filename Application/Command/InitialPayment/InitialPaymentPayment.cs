using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InitialPayment
{
    public class InitialPaymentPayment
    {
        public string? Name { get; set; }
        public string? Base64String { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentType { get; set; }
    }
}
