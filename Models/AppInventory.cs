using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppInventory : BaseEntity
    {
        public string? Name { get; set; }
        public int Quantity { get; set; } = 0;
        public AppInventoryType AppInventoryType { get; set; }
        public bool NotifyWhenLow { get; set; } = true;
        public int HowLow { get; set; } = 10;
        public string? Profile { get; set; }
        public string? SellType { get; set; }
        public ICollection<AppInventoryDependencies> Dependencies { get; set; } = new List<AppInventoryDependencies>();
        public ICollection<AppInventoryItem>? AppInventoryItems { get; set; } = new List<AppInventoryItem>();
        public ICollection<TicketInventory>? TicketInventories { get; set; } = new List<TicketInventory>();
    }
}
