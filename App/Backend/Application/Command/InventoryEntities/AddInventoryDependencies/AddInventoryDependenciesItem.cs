using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.AddInventoryDependencies
{
    public class AddInventoryDependenciesItem
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public int Amount { get; set; }
    }
}
