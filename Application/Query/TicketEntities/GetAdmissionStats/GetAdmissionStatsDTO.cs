using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.TicketEntities.GetAdmissionStats
{
    public class GetAdmissionStatsDTO
    {
        public Guid? AppTicketId { get; set; }
        public Patient? Patient { get; set; }
        public int? Pharmacy { get; set; }
        public int? Lab { get; set; }
        public int? Radiology { get; set; }
        public int? Surgery { get; set; }
        public ICollection<TicketInventory>? TicketInventories { get; set; }
    }
}
