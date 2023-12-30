using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Core
{
    public static class InsertDefaultAppSettings
    {
        public async static Task CreateAppSettings(AppDBContext context)
        {
            await CreateBillingSettings(context);
        }

        private async static Task CreateBillingSettings(AppDBContext context)
        {
            var billingSettings = await context.AppSettings.FirstOrDefaultAsync(x => x.Type == Models.Enums.AppSettingType.billings);

            if (billingSettings != null)
            {
                return;
            }

            var newBillingSettings = new AppSetting
            {
                Type = Models.Enums.AppSettingType.billings,
                Data = JsonConvert.SerializeObject(new AppSettingBilling())
            };

            await context.AddAsync<AppSetting>(newBillingSettings);
            await context.SaveChangesAsync();
        }
    }
}
