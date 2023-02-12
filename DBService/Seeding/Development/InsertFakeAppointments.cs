using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Development
{
    public static class InsertFakeAppointments
    {
        public async static Task CreateAppointments(AppDBContext context)
        {
            var oneInventoryItem = await context.AppAppointments.FirstOrDefaultAsync();

            if (oneInventoryItem != null)
                return;

            var patients = await context.Patients.Select(x => x.Id).ToArrayAsync();
            var staff = await context.Staff.Select(x => x.Id).ToArrayAsync();
            var companyIds = await context.Companies.Select(x => x.Id).ToArrayAsync();
            Random random = new Random();

            foreach (var patient in patients)
            {
                var appoitment = new AppAppointment
                {
                    PatientId = patient,
                    DoctorId = staff[random.Next(0, staff.Count())],
                    CompanyId = companyIds[random.Next(0, companyIds.Count())],
                    AppointmentDate = DateTime.Today.AddDays(random.Next(1, 31)).AddHours(random.Next(1, 23)).ToUniversalTime(),
                };

                await context.AppAppointments.AddAsync(appoitment);
            }
            await context.SaveChangesAsync();
        }
    }
}
