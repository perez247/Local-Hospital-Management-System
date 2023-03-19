using DBService.Seeding.Development.DevData;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Development
{
    public static class InsertFakeAppointments
    {
        public async static Task CreateAppointments(AppDBContext context, string initialDir)
        {
            var oneInventoryItem = await context.AppAppointments.FirstOrDefaultAsync();

            if (oneInventoryItem != null)
                return;

            var patients = await context.Patients.ToListAsync();
            var staff = await context.Staff.ToListAsync();
            var companyIds = await context.Companies.Select(x => x.Id).ToArrayAsync();
            Random random = new Random();

            var userDir = $"{initialDir}/tickets.json";

            //StreamReader jsonData = new StreamReader(Path.Combine(Path.GetFullPath(userDir)));
            var tickets = JsonConvert.DeserializeObject<List<AppTicket>>(TicketData.JsonData);

            var inventories = await context.AppInventories.ToListAsync();

            foreach (var patient in patients)
            {
                var appoitment = new AppAppointment
                {
                    Id = Guid.NewGuid(),
                    PatientId = patient.Id,
                    DoctorId = staff[random.Next(0, staff.Count())].Id,
                    CompanyId = patient.CompanyId,
                    AppointmentDate = DateTime.Today.AddDays(random.Next(1, 31)).AddHours(random.Next(1, 23)).ToUniversalTime(),
                };

                var ticketsToAdd = tickets.OrderBy(x => Guid.NewGuid()).Take(random.Next(10, 45)).ToList();

                var appointmentTickets = ticketsToAdd.Select(x =>
                {
                    var ticket = new AppTicket
                    {
                        Id = Guid.NewGuid(),
                        AppointmentId = appoitment.Id,
                        OverallDescription = x.OverallDescription,
                        AppTicketStatus = x.AppTicketStatus,
                        AppInventoryType = x.AppInventoryType,
                    };

                    ticket.TicketInventories = GenerateTickets(x.AppInventoryType, ticket.Id ?? Guid.Empty, inventories, patient, staff);

                    return ticket;
                }
                
                );

                await context.AppAppointments.AddAsync(appoitment);
                await context.AppTickets.AddRangeAsync(appointmentTickets);
            }
            await context.SaveChangesAsync();
        }

        public static ICollection<TicketInventory> GenerateTickets(AppInventoryType type, Guid tickeTId, ICollection<AppInventory> appInventories, Patient patient, List<Staff> staff)
        {
            switch (type)
            {
                case AppInventoryType.pharmacy:
                    return GeneratePharmacyTickets(tickeTId, appInventories);
                case AppInventoryType.surgery:
                    return GenerateSurgeryTickets(tickeTId, appInventories, patient, staff);
                case AppInventoryType.lab:
                    return GenerateLabOrRadiologyTickets(tickeTId, appInventories, type);
                case AppInventoryType.radiology:
                    return GenerateLabOrRadiologyTickets(tickeTId, appInventories, type);
                default:
                    return GenerateAdmissionTickets(tickeTId, appInventories);
            }
        }

        public static ICollection<TicketInventory> GeneratePharmacyTickets(Guid tickeTId, ICollection<AppInventory> appInventories)
        {
            Random random = new Random();
            var pharmacyInventory = appInventories.Where(x => x.AppInventoryType == AppInventoryType.pharmacy)
                                                                .OrderBy(a => Guid.NewGuid())
                                                                .Take(random.Next(1, 10))
                                                                .ToList();

            return pharmacyInventory.Select(x => new TicketInventory
            {
                AppTicketId = tickeTId,
                AppInventoryId = x.Id,
                DoctorsPrescription = "Sint proident nisi enim ex excepteur consequat eu eu. Eu excepteur et magna commodo pariatur et qui veniam amet. Est ullamco id aute reprehenderit. Elit tempor est dolore duis amet eu eu voluptate esse officia dolor est anim. Labore qui in exercitation laboris officia et reprehenderit id.\r\n",
                PrescribedQuantity = random.Next(1, 10).ToString()
            }).ToList();
        }

        public static ICollection<TicketInventory> GenerateSurgeryTickets(Guid tickeTId, ICollection<AppInventory> appInventories, Patient patient, ICollection<Staff> staff)
        {
            Random random = new Random();
            var pharmacyInventory = appInventories.Where(x => x.AppInventoryType == AppInventoryType.surgery)
                                                                .OrderBy(a => Guid.NewGuid())
                                                                .Take(random.Next(1, 2))
                                                                .ToList();

            return pharmacyInventory.Select(x =>
                {
                    var n = new TicketInventory
                    {
                        Id = Guid.NewGuid(),
                        AppTicketId = tickeTId,
                        AppInventoryId = x.Id,
                        DoctorsPrescription = "Sint proident nisi enim ex excepteur consequat eu eu. Eu excepteur et magna commodo pariatur et qui veniam amet. Est ullamco id aute reprehenderit. Elit tempor est dolore duis amet eu eu voluptate esse officia dolor est anim. Labore qui in exercitation laboris officia et reprehenderit id.\r\n",
                        SurgeryDate = DateTime.Today.AddDays(random.Next(1, 30)).AddHours(random.Next(1, 23)).ToUniversalTime(),
                    };

                    n.SurgeryTicketPersonnels = GetPersonals(patient, n.Id, staff.OrderBy(x => Guid.NewGuid()).ToList());

                    return n;
                }
            ).ToList();
        }

        private static ICollection<SurgeryTicketPersonnel> GetPersonals(Patient patient, Guid? ticketInventoryId, List<Staff> staff)
        {
            Random random = new Random();
            var personals = new List<SurgeryTicketPersonnel>();
            for (int i = 0; i < random.Next(2, 3); i++)
            {
                personals.Add(new SurgeryTicketPersonnel
                {
                    TicketInventoryId = ticketInventoryId,
                    PersonnelId = staff[i].AppUserId,
                    SurgeryRole = "Staff",
                    Description = "Sint proident nisi enim ex excepteur consequat eu eu. Eu excepteur et magna commodo pariatur et qui veniam amet. Est ullamco id aute reprehenderit. Elit tempor est dolore duis amet eu eu voluptate esse officia dolor est anim. Labore qui in exercitation laboris officia et reprehenderit id.\r\n",
                });
            }

            personals[random.Next(0, personals.Count - 1)].IsHeadPersonnel = true;

            personals.Add(new SurgeryTicketPersonnel
            {
                PersonnelId = patient.AppUserId,
                SurgeryRole = "Patient",
                Description = "Sint proident nisi enim ex excepteur consequat eu eu. Eu excepteur et magna commodo pariatur et qui veniam amet. Est ullamco id aute reprehenderit. Elit tempor est dolore duis amet eu eu voluptate esse officia dolor est anim. Labore qui in exercitation laboris officia et reprehenderit id.\r\n",
                IsPatient = true,
            });

            return personals;
        }

        public static ICollection<TicketInventory> GenerateLabOrRadiologyTickets(Guid tickeTId, ICollection<AppInventory> appInventories, AppInventoryType type)
        {
            Random random = new Random();
            var pharmacyInventory = appInventories.Where(x => x.AppInventoryType == type)
                                                                .OrderBy(a => Guid.NewGuid())
                                                                .Take(random.Next(1, 5))
                                                                .ToList();

            return pharmacyInventory.Select(x => new TicketInventory
            {
                AppTicketId = tickeTId,
                AppInventoryId = x.Id,
                DoctorsPrescription = "Sint proident nisi enim ex excepteur consequat eu eu. Eu excepteur et magna commodo pariatur et qui veniam amet. Est ullamco id aute reprehenderit. Elit tempor est dolore duis amet eu eu voluptate esse officia dolor est anim. Labore qui in exercitation laboris officia et reprehenderit id.\r\n",
                DateOfLabTest = DateTime.Today.AddDays(random.Next(1, 30)).AddHours(random.Next(1, 23)).ToUniversalTime(),
            }).ToList();
        }

        public static ICollection<TicketInventory> GenerateAdmissionTickets(Guid tickeTId, ICollection<AppInventory> appInventories)
        {
            Random random = new Random();
            var pharmacyInventory = appInventories.Where(x => x.AppInventoryType == AppInventoryType.admission)
                                                                .OrderBy(a => Guid.NewGuid())
                                                                .Take(random.Next(1, 1))
                                                                .ToList();

            return pharmacyInventory.Select(x => new TicketInventory
            {
                AppTicketId = tickeTId,
                AppInventoryId = x.Id,
                DoctorsPrescription = "Sint proident nisi enim ex excepteur consequat eu eu. Eu excepteur et magna commodo pariatur et qui veniam amet. Est ullamco id aute reprehenderit. Elit tempor est dolore duis amet eu eu voluptate esse officia dolor est anim. Labore qui in exercitation laboris officia et reprehenderit id.\r\n",
                AdmissionStartDate = DateTime.Today.AddDays(random.Next(0, 5)).AddHours(random.Next(1, 23)).ToUniversalTime(),
            }).ToList();
        }
    }
}
