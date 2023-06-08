using Application.Command.AppointmentEntities.AddAppointment;
using Application.Command.TicketEntities.AddPharmacyTicketInventory;
using Application.Command.TicketEntities.SaveTicketAndInventory;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Requests;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class TicketHelper
    {
        public static async Task<AppAppointment> CreateAppointment(AppAppointmentCommand request, IStaffRepository iStaffRepository, IPatientRepository iPatientRepository)
        {
            var doctor = await iStaffRepository.Staff().FirstOrDefaultAsync(x => x.Id.ToString() == request.DoctorId);

            var doctorId = doctor?.Id;

            var patient = await iPatientRepository.Patients()
                                                  .Include(x => x.PatientContracts.OrderByDescending(y => y.StartDate).Take(1))
                                                    .ThenInclude(x => x.AppCost)
                                                  .Include(x => x.Company)
                                                    .ThenInclude(x => x.CompanyContracts.OrderByDescending(y => y.StartDate).Take(1))
                                                        .ThenInclude(x => x.AppCost)
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.PatientId);


            if (patient == null)
            {
                throw new CustomMessageException("Patient to add to appointmet not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!patient.HasContract())
            {
                throw new CustomMessageException("Patient does not have any contract at the moment");
            }

            var time = request.AppointmentDate.Value.ToUniversalTime().Hour;

            if (time >= 22 || time <= 2)
            {
                throw new CustomMessageException("Hour must be between 3AM and 9PM");
            }

            var appointment = new AppAppointment
            {
                DoctorId = doctorId,
                PatientId = patient?.Id,
                CompanyId = patient?.CompanyId,
                AppointmentDate = request.AppointmentDate.Value.ToUniversalTime()
            };


            return appointment;
        }

        public static async Task AddOrUpdateExistingTickets(
            SaveTicketAndInventoryCommand request,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository

            )
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                                      .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            if (ticketFromDb == null)
            {

                var totalAppointmentTickets = await iTicketRepository.AppTickets().CountAsync(x => x.AppointmentId.ToString() == request.AppointmentId);

                if (totalAppointmentTickets >= 20)
                {
                    throw new CustomMessageException("Only a maximum of 20 tickets per appointment");
                }

                ticketFromDb = new AppTicket
                {
                    Id = Guid.NewGuid(),
                    AppInventoryType = request.AppInventoryType.ParseEnum<AppInventoryType>(),
                    AppointmentId = request.AppointmentId != Guid.Empty.ToString() ? Guid.Parse(request.AppointmentId) : null,
                };
                ticketFromDb.OverallDescription = request.OverallDescription.Trim();
                await iDBRepository.AddAsync<AppTicket>(ticketFromDb);
            }
            else
            {
                ticketFromDb.OverallDescription = request.OverallDescription.Trim();
                iDBRepository.Update<AppTicket>(ticketFromDb);
            }

            ticketFromDb.MustNotHaveBeenSentToDepartment();

            request.TicketInventories = request.TicketInventories.DistinctBy(x => x.InventoryId).ToList();

            var ids = request.TicketInventories.Select(x => x.InventoryId);

            if (ids.Count() >= 30)
            {
                throw new CustomMessageException("Only a maximum of 20 inventory per ticket");
            }

            var inventories = await iInventoryRepository.AppInventories()
                                                        .Where(x => ids.Contains(x.Id.ToString()))
                                                        .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                               .Where(x => x.AppTicketId == ticketFromDb.Id)
                                               .ToListAsync();

            var ticketsInventoriesUpdated = new List<Guid>();

            foreach (var ticketInventory in request.TicketInventories)
            {
                await TicketHelper.SaveTicketInventory(request, ticketInventory, ticketFromDb, inventories, ticketInventories, iDBRepository, ticketsInventoriesUpdated);
            }

            // Delete any ticket that was not found
            var ticketsInventoriesToBeDeleted = ticketInventories.Where(x => !ticketsInventoriesUpdated.Contains(x.Id.Value));

            if (ticketsInventoriesToBeDeleted.Count() > 0)
            {
                foreach (var inventory in ticketsInventoriesToBeDeleted)
                {
                    iDBRepository.Remove<TicketInventory>(inventory);
                }
            }
        }

        private static async Task SaveTicketInventory(
            SaveTicketAndInventoryCommand command,
            SaveTicketAndInventoryRequest request,
            AppTicket? ticketFromDb,
            List<AppInventory> appInventories,
            List<TicketInventory> ticketInventories,
            IDBRepository iDBRepository,
            ICollection<Guid> ticketsInventoriesUpdated
            )
        {
            var inventory = appInventories.FirstOrDefault(x => x.Id.ToString() == request.InventoryId);

            if (inventory == null)
            {
                throw new CustomMessageException($"{command.AppInventoryType} to add not found");
            }

            var hasPharmacyInventory = ticketInventories.FirstOrDefault(x => x.Id.ToString() == request.InventoryId);

            if (hasPharmacyInventory != null)
            {
                hasPharmacyInventory.DoctorsPrescription = string.IsNullOrEmpty(request.DoctorsPrescription) ? null : request.DoctorsPrescription;
                hasPharmacyInventory.PrescribedQuantity = string.IsNullOrEmpty(request.PrescribedQuantity) ? null : request.PrescribedQuantity;
                iDBRepository.Update<TicketInventory>(hasPharmacyInventory);
            }
            else
            {
                hasPharmacyInventory = new TicketInventory
                {
                    Id = Guid.NewGuid(),
                    AppInventoryId = inventory.Id,
                    AppTicketId = ticketFromDb.Id,
                    DoctorsPrescription = string.IsNullOrEmpty(request.DoctorsPrescription) ? null : request.DoctorsPrescription,
                    PrescribedQuantity = string.IsNullOrEmpty(request.PrescribedQuantity) ? null : request.PrescribedQuantity,
                };

                await iDBRepository.AddAsync<TicketInventory>(hasPharmacyInventory);
            }

            ticketsInventoriesUpdated.Add(hasPharmacyInventory.Id.Value);
        }

        public static async Task VerifyInventories(
            ICollection<SendTicketToFinanceRequest> requestTicketInventories,
            AppTicket? ticketFromDb,
            IEnumerable<string?> inventoryIds,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository,
            Func<SendTicketToFinanceRequest?, AppInventory?, object>? moreValidation
            )
        {
            var inventories = await iInventoryRepository.AppInventories()
                                            .Where(x => inventoryIds.Contains(x.Id.ToString()))
                                            .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                                           .Where(x => x.AppTicketId == ticketFromDb.Id)
                                                           .ToListAsync();

            foreach (var itemTicket in requestTicketInventories)
            {
                var inventory = inventories.FirstOrDefault(x => x.Id.ToString() == itemTicket.InventoryId);

                if (inventory == null)
                {
                    throw new CustomMessageException("Item not recorded in the inventory");
                }

                var pharmacyTicketInventory = ticketInventories.FirstOrDefault(x => x.Id.ToString() == itemTicket.TicketInventoryId);

                if (pharmacyTicketInventory == null)
                {
                    throw new CustomMessageException("Item not found for this ticket");
                }

                if (moreValidation != null)
                {
                    moreValidation(itemTicket, inventory);
                }

                pharmacyTicketInventory.PrescribedQuantity = itemTicket.PrescribedQuantity.ToString();
                pharmacyTicketInventory.DepartmentDescription = itemTicket.DepartmentDescription;
                pharmacyTicketInventory.AppTicketStatus = itemTicket.AppTicketStatus.ParseEnum<AppTicketStatus>();

                iDBRepository.Update(pharmacyTicketInventory);
            }
        }

        public static async Task<AppTicket?> BasicConclusion<T>(
            string ticketId, 
            ICollection<T>? ConcludeTicketRequest,
            ITicketRepository iTicketRepository,
            IDBRepository iDBRepository,
            Boolean trackForSaving = true
            ) where T : ConcludeTicketRequest
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                          .Include(x => x.TicketInventories)
                                            .ThenInclude(x => x.AppInventory)
                                          .Include(x => x.AppCost)
                                          .FirstOrDefaultAsync(x => x.Id.ToString() == ticketId);

            ticketFromDb.MustHaveBeenSentToFinance();

            var appCost = ticketFromDb.AppCost;
            if (appCost.PaymentStatus == PaymentStatus.pending)
            {
                throw new CustomMessageException("Payment status should not be pending");
            }

            ConcludeTicketRequest = ConcludeTicketRequest.DistinctBy(x => x.InventoryId).ToList();


            var ticketInventories = ticketFromDb.TicketInventories;

            foreach (var genericTicketInventory in ConcludeTicketRequest)
            {

                var ticketInventory = ticketInventories.FirstOrDefault(x => x.Id.ToString() == genericTicketInventory.InventoryId);

                if (ticketInventory == null)
                {
                    throw new CustomMessageException("Item not found for this ticket");
                }

                if (ticketInventory.AppInventory == null)
                {
                    throw new CustomMessageException("Item not found in the inventory");
                }

                ticketInventory.ConcludedDate = genericTicketInventory.ConcludedDate;
                ticketInventory.AppTicketStatus = genericTicketInventory.AppTicketStatus.ParseEnum<AppTicketStatus>();
                ticketInventory.Proof = genericTicketInventory.Proof;

                if (trackForSaving)
                {
                    iDBRepository.Update(ticketInventory);
                }

            }

            return ticketFromDb;
        }
    }
}
