using Application.Annotations;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.SaveInventoryItem
{
    public class SaveInventoryItemResponse
    {
        public string? InventoryId { get; set; }
        public string? CompanyId { get; set; }
        public string? InventoryItemId { get; set; }
        public decimal? NewPrice { get; set; }
        public int Index { get; set; }

        public static SaveInventoryItemResponse Create(AppInventoryItem AppInventoryItem)
        {
            var data = new SaveInventoryItemResponse
            {
                InventoryId = AppInventoryItem.AppInventoryId.ToString(),
                CompanyId = AppInventoryItem.CompanyId.ToString(),
                InventoryItemId = AppInventoryItem.Id.ToString(),
                NewPrice = AppInventoryItem.PricePerItem,
            };
            return data;
        }
    }
}
