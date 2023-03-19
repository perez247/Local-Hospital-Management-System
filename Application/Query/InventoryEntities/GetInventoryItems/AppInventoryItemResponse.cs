using Application.Query.InventoryEntities.GetInventories;
using Application.Responses;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetInventoryItems
{
    public class AppInventoryItemResponse
    {
        public BaseResponse? Base { get; set; }
        public CompanyResponse? Company { get; set; }
        public InventoryResponse? Inventory { get; set; }
        public decimal PricePerItem { get; set; }

        public static AppInventoryItemResponse? Create(AppInventoryItem appInventoryItem)
        {
            if (appInventoryItem == null)
            {
                return null;
            }

            return new AppInventoryItemResponse
            {
                Base = BaseResponse.Create(appInventoryItem),
                Company = CompanyResponse.Create(appInventoryItem.Company),
                Inventory = InventoryResponse.Create(appInventoryItem.AppInventory),
                PricePerItem = appInventoryItem.PricePerItem,
            };
        }
    }
}
