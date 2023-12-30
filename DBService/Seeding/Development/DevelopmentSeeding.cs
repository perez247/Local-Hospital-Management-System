using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Development
{
    public static class DevelopmentSeeding
    {
        private static string initialDir = "./InitialData/Dev";

        public static async Task BeginSeeding(AppDBContext context, UserManager<AppUser> userManager)
        {
            

            // Users
            await InsertFakeUsers.CreateStaff(context, userManager, initialDir);
            await InsertFakeUsers.CreateCompany(context, userManager, initialDir);
            await InsertFakeUsers.CreatePatient(context, userManager, initialDir);
            await InsertFakeUsers.CreatePatientInCompany(context, userManager, initialDir);

            // Inventory..
            await InsertFakeInventories.CreateInventories(context, initialDir);
            await InsertFakeInventories.CreateInventoryItems(context, initialDir);

            // Appointments
            await InsertFakeAppointments.CreateAppointments(context, initialDir);

            // await context.SaveChangesAsync();
        }
    }
}
