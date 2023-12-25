using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Constants;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;
using Application.Utilities;
using System.ComponentModel.Design;
using DBService.Seeding.Development.DevData;

namespace DBService.Seeding.Development
{
    public static class InsertFakeInventories
    {
        public async static Task CreateInventories(AppDBContext context, string initialDir)
        {
            var oneInventory = await context.AppInventories.FirstOrDefaultAsync();

            if (oneInventory != null)
                return;

            var inventoryTypes = Enum.GetNames(typeof(AppInventoryType));
            var random = new Random();

            var userDir = $"{initialDir}/inventory.json";

            //using (StreamReader jsonData = new StreamReader(Path.Combine(Path.GetFullPath(userDir))))
            //{
                var inventories = JsonConvert.DeserializeObject<List<AppInventory>>(InventoryData.JsonData);

                foreach (var inventory in inventories)
                {
                    var index = random.Next(0, inventoryTypes.Count());
                    var newInventory = new AppInventory
                    {
                        Id = inventory.Id,
                        Name = inventory.Name,
                        Quantity = inventory.Quantity,
                        NotifyWhenLow = inventory.NotifyWhenLow,
                        HowLow = inventory.HowLow,
                        AppInventoryType = inventoryTypes[index].ParseEnum<AppInventoryType>(),
                        //AppInventoryType = AppInventoryType.pharmacy,
                    };

                    await context.AddAsync<AppInventory>(newInventory);
                }

                await context.SaveChangesAsync();
            //}

            InventoryData.JsonData = "";

        }
        public async static Task CreateInventoryItems(AppDBContext context, string initialDir)
        {
            var oneInventoryItem = await context.AppInventoryItems.FirstOrDefaultAsync();

            if (oneInventoryItem != null)
                return;

            var companyIds = await context.Companies.Select(x => x.Id).ToArrayAsync();
            var appInventoryIds = await context.AppInventories.Select(x => x.Id).ToArrayAsync();
            Random random = new Random();

            foreach (var appInventoryId in appInventoryIds)
            {
                foreach (var companyId in companyIds)
                {
                    var newAppInventoryItem = new AppInventoryItem
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = companyId,
                        AppInventoryId = appInventoryId,
                        PricePerItem = (decimal)Math.Round((random.NextDouble() * 10001), 2),
                    };
                    await context.AddAsync<AppInventoryItem>(newAppInventoryItem);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
