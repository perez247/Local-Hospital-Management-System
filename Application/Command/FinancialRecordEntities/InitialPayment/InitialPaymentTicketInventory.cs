using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.FinancialRecordEntities.InitialPayment
{
    public class InitialPaymentTicketInventory
    {
        [VerifyGuidAnnotation]
        public string? TicketInventoryId { get; set; }
        public string? AppTicketStatus { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? PrescribedQuantity { get; set; }
    }
}
