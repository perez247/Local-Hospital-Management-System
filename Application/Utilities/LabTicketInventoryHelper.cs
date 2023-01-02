using Application.Command.AddAppointment;
using Application.Command.AddLabTicketInventory;
using Application.Command.UpdateLabTicketInventory;
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
    public static class LabTicketInventoryHelper
    {
        private static async Task<TicketInventory> SaveLabTicketInventory(
            AddLabTicketInventoryRequest request,
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

            var hasLabInventory = ticketInventories.FirstOrDefault(x => x.AppTicketId == ticketFromDb.Id);

            if (hasLabInventory != null)
            {
                hasLabInventory.PrescribedLabRadiologyFeature = string.IsNullOrEmpty(request.PrescribedLabRadiologyFeature) ? null : request.PrescribedLabRadiologyFeature;
                hasLabInventory.CurrentPrice = request.CurrentPrice;
                iDBRepository.Update<TicketInventory>(hasLabInventory);
                return hasLabInventory;
            }
            else
            {
                var newLabInventory = new TicketInventory
                {
                    AppInventoryId = inventory.Id,
                    AppTicketId = ticketFromDb.Id,
                    PrescribedLabRadiologyFeature = string.IsNullOrEmpty(request.PrescribedLabRadiologyFeature) ? null : request.PrescribedLabRadiologyFeature,
                    CurrentPrice = request.CurrentPrice,
                    TotalPrice = request.CurrentPrice,
                };

                await iDBRepository.AddAsync<TicketInventory>(newLabInventory);
                return newLabInventory;
            }
        }

        public static async Task<List<TicketInventory>> AddNewOrExistingLabTickets(
            AddLabTicketInventoryCommand request,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository
            )
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustNotHaveBeenSentToDepartment();

            request.AddLabTicketInventoryRequest = request.AddLabTicketInventoryRequest.DistinctBy(x => x.InventoryId).ToList();

            var ids = request.AddLabTicketInventoryRequest.Select(x => x.InventoryId);

            var inventories = await iInventoryRepository.AppInventories().Where(x => ids.Contains(x.Id.ToString()))
                                                        .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                               .Where(x => x.AppTicketId == ticketFromDb.Id)
                                               .ToListAsync();
            var newItem = new List<TicketInventory>();
            foreach (var ticketInventory in request.AddLabTicketInventoryRequest)
            {
                var item = await LabTicketInventoryHelper.SaveLabTicketInventory(ticketInventory, ticketFromDb, inventories, ticketInventories, iDBRepository);
                newItem.Add(item);
            }

            return newItem;
        }

        public static async Task<List<TicketInventory>> UpdateLabTickets(
            UpdateLabTicketInventoryCommand request,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository
            )
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                                      .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustHvaeBeenSentToDepartment();

            request.UpdateLabTicketInventoryRequest = request.UpdateLabTicketInventoryRequest.DistinctBy(x => x.InventoryId).ToList();


            var inventoryIds = request.UpdateLabTicketInventoryRequest.Select(y => y.InventoryId);

            var inventories = await iInventoryRepository.AppInventories()
                                                        .Where(x => inventoryIds.Contains(x.Id.ToString()))
                                                        .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                                           .Where(x => x.AppTicketId == ticketFromDb.Id)
                                                           .ToListAsync();

            var newItem = new List<TicketInventory>();
            foreach (var genericTicket in request.UpdateLabTicketInventoryRequest)
            {
                var inventory = inventories.FirstOrDefault(x => x.Id.ToString() == genericTicket.InventoryId);

                if (inventory == null)
                {
                    throw new CustomMessageException("Drug not recorded in the inventory");
                }

                var pharmacyTicketInventory = ticketInventories.FirstOrDefault(x => x.AppTicketId == ticketFromDb.Id);

                if (pharmacyTicketInventory == null)
                {
                    throw new CustomMessageException("Drug not found for this ticket");
                }

                pharmacyTicketInventory.DateOfLabTest = genericTicket.DateOfLabTest;

                newItem.Add(pharmacyTicketInventory);

                iDBRepository.Update<TicketInventory>(pharmacyTicketInventory);
            }

            return newItem;
        }
    }
}
