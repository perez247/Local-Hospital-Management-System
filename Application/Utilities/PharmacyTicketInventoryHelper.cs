using Application.Command.AddPharmacyTicketInventory;
using Application.Command.UpdatePharmacyTicketInventory;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class PharmacyTicketInventoryHelper
    {
        private static async Task SavePharmacyTicketInventory(
            AddPharmacyTicketRequest request,
            AppTicket? ticketFromDb,
            List<AppInventory> appInventories,
            List<TicketInventory> ticketInventories,
            IDBRepository iDBRepository
            )
        {
            var inventory = appInventories.FirstOrDefault(x => x.Id.ToString() == request.InventoryId);

            if (inventory == null)
            {
                throw new CustomMessageException("Pharmacy to add not found");
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
                var pharmacyInventory = new TicketInventory
                {
                    AppInventoryId = inventory.Id,
                    AppTicketId = ticketFromDb.Id,
                    DoctorsPrescription = string.IsNullOrEmpty(request.DoctorsPrescription) ? null : request.DoctorsPrescription,
                    PrescribedQuantity = string.IsNullOrEmpty(request.PrescribedQuantity) ? null : request.PrescribedQuantity,
                };

                await iDBRepository.AddAsync<TicketInventory>(pharmacyInventory);
            }
        }

        public static async Task AddOrUpdateExistingPharmacyTickets(
            AddPharmacyTicketInventoryCommand request,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository

            )
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);
            
            if (ticketFromDb == null)
            {
                ticketFromDb = new AppTicket
                {
                    Id = Guid.NewGuid(),
                    AppointmentId = request.AppointmentId != Guid.Empty.ToString() ? Guid.Parse(request.AppointmentId) : null,
                };
                ticketFromDb.OverallDescription = request.OverallDescription.Trim();
                await iDBRepository.AddAsync<AppTicket>(ticketFromDb);
            } else
            {
                ticketFromDb.OverallDescription = request.OverallDescription.Trim();
                iDBRepository.Update<AppTicket>(ticketFromDb);
            }

            ticketFromDb.MustNotHaveBeenSentToDepartment();

            request.TicketInventories = request.TicketInventories.DistinctBy(x => x.InventoryId).ToList();

            var ids = request.TicketInventories.Select(x => x.InventoryId);

            var inventories = await iInventoryRepository.AppInventories()
                                                        .Where(x => ids.Contains(x.Id.ToString()))
                                                        .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                               .Where(x => x.AppTicketId == ticketFromDb.Id)
                                               .ToListAsync();

            foreach (var ticketInventory in request.TicketInventories)
            {
                await PharmacyTicketInventoryHelper.SavePharmacyTicketInventory(ticketInventory, ticketFromDb, inventories, ticketInventories, iDBRepository);
            }
        }

        public static async Task UpdatePharmacyTickets(
            UpdatePharmacyTicketInventoryCommand request,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository
            )
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                                      .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustHvaeBeenSentToDepartment();

            request.UpdatePharmacyTicketInventoryRequest = request.UpdatePharmacyTicketInventoryRequest.DistinctBy(x => x.InventoryId).ToList();


            var inventoryIds = request.UpdatePharmacyTicketInventoryRequest.Select(y => y.InventoryId);

            var inventories = await iInventoryRepository.AppInventories()
                                                        .Where(x => inventoryIds.Contains(x.Id.ToString()))
                                                        .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                                           .Where(x => x.AppTicketId == ticketFromDb.Id)
                                                           .ToListAsync();

            foreach (var pharmacyTicket in request.UpdatePharmacyTicketInventoryRequest)
            {
                var inventory = inventories.FirstOrDefault(x => x.Id.ToString() == pharmacyTicket.InventoryId);

                if (inventory == null)
                {
                    throw new CustomMessageException("Drug not recorded in the inventory");
                }

                var pharmacyTicketInventory = ticketInventories.FirstOrDefault(x => x.AppTicketId == ticketFromDb.Id);

                if (pharmacyTicketInventory == null)
                {
                    throw new CustomMessageException("Drug not found for this ticket");
                }

                pharmacyTicketInventory.AppInventoryQuantity = pharmacyTicket.AppInventoryQuantity;
                pharmacyTicketInventory.CurrentPrice = pharmacyTicketInventory.CurrentPrice;
                pharmacyTicketInventory.TotalPrice = pharmacyTicket.AppInventoryQuantity * pharmacyTicketInventory.CurrentPrice;
                pharmacyTicketInventory.StaffObservation = pharmacyTicket.StaffObservation;
                pharmacyTicketInventory.Description = pharmacyTicket.Description;

                //pharmacyTicketInventory.ConcludedDate = pharmacyTicket.ConcludedDate;
                //pharmacyTicketInventory.Proof = pharmacyTicket.Proof;

                iDBRepository.Update<TicketInventory>(pharmacyTicketInventory);

            }
        }
    }
}
