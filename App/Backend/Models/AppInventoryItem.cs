using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppInventoryItem : BaseEntity
    {
        public virtual Company? Company { get; set; }
        public Guid? CompanyId { get; set; }
        public virtual AppInventory? AppInventory { get; set; }
        public Guid? AppInventoryId { get; set; }
        public decimal PricePerItem { get; set; }
    }
}
