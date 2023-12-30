using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class InventoryResponse
    {
        public BaseResponse? Base { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public string? AppInventoryType { get; set; }
        public bool NotifyWhenLow { get; set; }
        public int HowLow { get; set; }
        public string? Profile { get; set; }
        public IEnumerable<InventoryOnlyResponse>? Dependencies { get; set; }

        public static InventoryResponse? Create(AppInventory appInventory)
        {
            if (appInventory == null)
            {
                return null;
            }

            return new InventoryResponse
            {
                Base = BaseResponse.Create(appInventory),
                Name = appInventory.Name,
                Quantity = appInventory.Quantity,
                AppInventoryType = appInventory.AppInventoryType.ToString(),
                NotifyWhenLow = appInventory.NotifyWhenLow,
                HowLow = appInventory.HowLow,
                Profile = appInventory.Profile,
                Dependencies = appInventory.Dependencies.Select(x => InventoryOnlyResponse.Create(x.AppInventory))
            };
        }
    }
}
