using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppInventoryDependencies : BaseEntity
    {
        public Guid AppInventoryId { get; set; }
        public AppInventory? AppInventory { get; set; }
        public Guid DependantId { get; set; }
        public AppInventory? Dependant { get; set; }
        public int DefaultAmount { get; set; } = 0;
    }
}
