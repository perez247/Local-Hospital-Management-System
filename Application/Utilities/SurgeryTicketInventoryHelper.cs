using Application.Command.SaveSurgeryTicketInventory;
using Application.Command.UpdateSurgeryTicketInventory;
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
    public static class SurgeryTicketInventoryHelper
    {

        private static async Task<TicketInventory> AddSurgeryTicket(
            SaveSurgeryTicketInventoryCommand request,
            AppTicket? ticketFromDb,
            List<AppInventory> inventories,
            SaveSurgeyTicketInventoryRequest surgeryRequest,
            ITicketRepository iTicketRepository,
            IDBRepository iDBRepository
            )
        {
            var inventory = inventories.FirstOrDefault(x => x.Id.ToString() == surgeryRequest.InventoryId);

            if (inventory == null)
            {
                throw new CustomMessageException("Surgery not recorded in the inventory");
            }

            var surgeryTicketInventory = await iTicketRepository.TicketInventory()
                                                                .Include(x => x.SurgeryTicketPersonnels)
                                                                .FirstOrDefaultAsync(x => x.AppTicketId.ToString() == request.TicketId && x.AppInventoryId == inventory.Id);

            var surgeryTicketInventoryId = Guid.NewGuid();
            if (surgeryTicketInventory == null)
            {
                var newSurgeryTicketInventory = new TicketInventory
                {
                    Id = surgeryTicketInventoryId,
                    AppTicketId = ticketFromDb.Id,
                    AppInventoryId = inventory.Id,
                    PrescribedSurgeryDescription = surgeryRequest.PrescribedSurgeryDescription,
                    CurrentPrice = surgeryRequest.CurrentPrice,
                    TotalPrice = surgeryRequest.CurrentPrice,
                };
                await iDBRepository.AddAsync<TicketInventory>(newSurgeryTicketInventory);
                return newSurgeryTicketInventory;

            }
            else
            {
                surgeryTicketInventoryId = surgeryTicketInventory.Id.Value;
                surgeryTicketInventory.PrescribedSurgeryDescription = surgeryRequest.PrescribedSurgeryDescription;
                surgeryTicketInventory.CurrentPrice = surgeryRequest.CurrentPrice;
                surgeryTicketInventory.TotalPrice = surgeryRequest?.CurrentPrice;
                iDBRepository.Update<TicketInventory>(surgeryTicketInventory);
                return surgeryTicketInventory;
            }
        }

        public static async Task<List<TicketInventory>> AddNewOrExistingSurgerys(
            SaveSurgeryTicketInventoryCommand request,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository
            )
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);
            ticketFromDb.MustNotHaveBeenSentToDepartment();

            request.SaveSurgeyTicketInventoryRequest = request.SaveSurgeyTicketInventoryRequest.DistinctBy(x => x.InventoryId).ToList();


            var inventoryIds = request.SaveSurgeyTicketInventoryRequest.Select(y => y.InventoryId);

            var inventories = await iInventoryRepository.AppInventories()
                                                        .Where(x => inventoryIds.Contains(x.Id.ToString()))
                                                        .ToListAsync();
            var newItem = new List<TicketInventory>();
            foreach (var surgeryRequest in request.SaveSurgeyTicketInventoryRequest)
            {
                var item = await SurgeryTicketInventoryHelper.AddSurgeryTicket(request, ticketFromDb, inventories, surgeryRequest, iTicketRepository, iDBRepository);
                newItem.Add(item);
            }
            return newItem;
        }

        private static async Task UpdateNewOrExistingPersonnel(
            UpdateSurgeryTicketInventoryRequest surgeryRequest,
            Guid SurgerTicketInventoryId,
            IDBRepository iDBRepository
            )
        {
            foreach (var personnel in surgeryRequest.UpdateSurgeryTicketInventoryPersonnel)
            {
                var newPersonnel = new SurgeryTicketPersonnel
                {
                    TicketInventoryId = SurgerTicketInventoryId,
                    PersonnelId = Guid.Parse(personnel.UserId),
                    SurgeryRole = personnel.SurgeryRole,
                    Description = personnel.Description,
                    IsHeadPersonnel = personnel.IsHeadPersonnel,
                    IsPatient = personnel.IsPatient,
                };
                await iDBRepository.AddAsync<SurgeryTicketPersonnel>(newPersonnel);
            }
        }

        private static async Task UpdateSurgeryTicket(
            UpdateSurgeryTicketInventoryCommand request,
            AppTicket? ticketFromDb,
            List<AppInventory> inventories,
            UpdateSurgeryTicketInventoryRequest surgeryRequest,
            ITicketRepository iTicketRepository,
            IDBRepository iDBRepository
            )
        {
            var inventory = inventories.FirstOrDefault(x => x.Id.ToString() == surgeryRequest.InventoryId);

            if (inventory == null)
            {
                throw new CustomMessageException("Surgery not recorded in the inventory");
            }

            var surgeryTicketInventory = await iTicketRepository.TicketInventory()
                                                                .Include(x => x.SurgeryTicketPersonnels)
                                                                .FirstOrDefaultAsync(x => x.AppTicketId.ToString() == request.TicketId && x.AppInventoryId == inventory.Id);

            var surgeryTicketInventoryId = Guid.NewGuid();
            if (surgeryTicketInventory == null)
            {
                var newSurgeryTicketInventory = new TicketInventory
                {
                    Id = surgeryTicketInventoryId,
                    AppTicketId = ticketFromDb.Id,
                    AppInventoryId = inventory.Id,
                    SurgeryDate = surgeryRequest.SurgeryDate,
                };
                await iDBRepository.AddAsync<TicketInventory>(newSurgeryTicketInventory);
                await SurgeryTicketInventoryHelper.UpdateNewOrExistingPersonnel(surgeryRequest, surgeryTicketInventoryId, iDBRepository);
            }
            else
            {
                surgeryTicketInventoryId = surgeryTicketInventory.Id.Value;
                surgeryTicketInventory.SurgeryDate = surgeryRequest.SurgeryDate;
                iDBRepository.Update<TicketInventory>(surgeryTicketInventory);

                iDBRepository.RemoveRange<SurgeryTicketPersonnel>(surgeryTicketInventory.SurgeryTicketPersonnels);

                await SurgeryTicketInventoryHelper.UpdateNewOrExistingPersonnel(surgeryRequest, surgeryTicketInventoryId, iDBRepository);
            }
        }

        public static async Task UpdateSurgeryTicketList(
            UpdateSurgeryTicketInventoryCommand request,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository
            )
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                                      .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustHvaeBeenSentToDepartment();

            request.UpdateSurgeryTicketInventoryRequest = request.UpdateSurgeryTicketInventoryRequest.DistinctBy(x => x.InventoryId).ToList();


            var inventoryIds = request.UpdateSurgeryTicketInventoryRequest.Select(y => y.InventoryId);

            var inventories = await iInventoryRepository.AppInventories()
                                                        .Where(x => inventoryIds.Contains(x.Id.ToString()))
                                                        .ToListAsync();

            foreach (var req in request.UpdateSurgeryTicketInventoryRequest)
            {
                await SurgeryTicketInventoryHelper.UpdateSurgeryTicket(request, ticketFromDb, inventories, req, iTicketRepository, iDBRepository);
            }
        }
    }
}
