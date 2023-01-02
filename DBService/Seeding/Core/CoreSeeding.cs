using Microsoft.AspNetCore.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Core
{
    public static class CoreSeeding
    {
        public static async Task BeginSeeding(AppDBContext context, RoleManager<AppRole> roleManager)
        {
            await InsertRoles.CreateOrUpdate(context, roleManager);

            await context.SaveChangesAsync();
        }
    }
}
