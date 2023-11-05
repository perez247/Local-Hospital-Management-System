using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Command.TicketEntities.SaveTicketAndInventory;
using Application.Command.AdmissionEntities.SaveAdmissionPrescription;

namespace Application.Utilities
{
    public static class PrescriptionHelper
    {
        public static async Task<AdmissionPrescription> AddOrUpdateExistingAdmissionPrescription(
            SaveAdmissionPrescriptionCommand request,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository
        )
        {
            // Get the prescriptio
            //var prescriptionFromDB = await iTicketRepository.TicketPrescription()
            //                                          .FirstOrDefaultAsync(x => x.Id.ToString() == request.PrescriptionId);

            var secondPrescriptionFromDB = await iTicketRepository.TicketPrescription()
                                                                  .Where(x => x.Id.ToString() == request.PrescriptionId || x.AppTicketId.ToString() == request.TicketId)
                                                                  .OrderByDescending(x => x.DateCreated)
                                                                  .Take(2)
                                                                  .ToListAsync();

            var prescriptionFromDB = secondPrescriptionFromDB.FirstOrDefault(x => x.Id.ToString() == request.PrescriptionId);
            var previousPrescriptionFromDB = secondPrescriptionFromDB.FirstOrDefault(x => x.Id.ToString() != request.PrescriptionId);

            // check if prescription exist
            if (prescriptionFromDB == null)
            {
                if (previousPrescriptionFromDB != null && previousPrescriptionFromDB.AppTicketStatus == AppTicketStatus.ongoing)
                {
                    throw new CustomMessageException("Previous Prescription has to be concluded first");
                }

                prescriptionFromDB = new AdmissionPrescription
                {
                    Id = Guid.NewGuid(),
                    DoctorId = request.getCurrentUserRequest().CurrentUser.Id,
                    AppTicketId = Guid.Parse(request.TicketId),
                    OverallDescription = request.OverallDescription,
                    AppInventoryType = request.AppInventoryType.ParseEnum<AppInventoryType>(),
                };

                await iDBRepository.AddAsync<AdmissionPrescription>(prescriptionFromDB);
            } else
            {
                if (prescriptionFromDB.AppTicketStatus == AppTicketStatus.concluded)
                {
                    throw new CustomMessageException("You cannot update an ongoing prescription");
                }

                if (prescriptionFromDB.AppTicketStatus == AppTicketStatus.canceled)
                {
                    throw new CustomMessageException("You cannot update a closed prescription");
                }

                var newStatus = request.AppTicketStatus.ParseEnum<AppTicketStatus>(); ;

                if (newStatus == AppTicketStatus.concluded && previousPrescriptionFromDB != null)
                {
                    if (previousPrescriptionFromDB.AppTicketStatus == AppTicketStatus.concluded)
                    {
                        previousPrescriptionFromDB.AppTicketStatus = AppTicketStatus.canceled;
                        iDBRepository.Update<AdmissionPrescription>(previousPrescriptionFromDB);
                    }
                }

                prescriptionFromDB.OverallDescription = request.OverallDescription;
                prescriptionFromDB.DoctorId = request.getCurrentUserRequest().CurrentUser.Id;
                prescriptionFromDB.AppTicketStatus = newStatus;
                iDBRepository.Update<AdmissionPrescription>(prescriptionFromDB);
            }

            if (request.TicketInventories.Count == 0) { return prescriptionFromDB; }

            request.TicketInventories = request.TicketInventories.DistinctBy(x => x.InventoryId).ToList();

            var ids = request.TicketInventories.Select(x => x.InventoryId);

            if (ids.Count() >= 20)
            {
                throw new CustomMessageException("Only a maximum of 20 inventory per prescription");
            }

            var inventories = await iInventoryRepository.AppInventories()
                                                        .Where(x => ids.Contains(x.Id.ToString()))
                                                        .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                               .Where(x => x.AdmissionPrescriptionId == prescriptionFromDB.Id)
                                               .ToListAsync();

            var ticketsInventoriesUpdated = new List<Guid>();

            foreach (var ticketInventory in request.TicketInventories)
            {
                await SaveTicketInventory(request, ticketInventory, prescriptionFromDB, inventories, ticketInventories, iDBRepository, ticketsInventoriesUpdated);
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

            return prescriptionFromDB;
        }

        private static async Task SaveTicketInventory(
            SaveAdmissionPrescriptionCommand command,
            SaveAdmissionPrescriptionRequest request,
            AdmissionPrescription? prescriptionFromDb,
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

            var hasInventory = ticketInventories.FirstOrDefault(x => x.Id.ToString() == request.TicketInventoryId);

            if (hasInventory != null)
            {
                hasInventory.DoctorsPrescription = string.IsNullOrEmpty(request.DoctorsPrescription) ? null : request.DoctorsPrescription;
                hasInventory.PrescribedQuantity = string.IsNullOrEmpty(request.PrescribedQuantity) ? null : request.PrescribedQuantity;
                hasInventory.Times = request.Times;
                hasInventory.Dosage = request.Dosage;
                hasInventory.Frequency = request.Frequency;
                hasInventory.Duration = request.Duration;
                iDBRepository.Update<TicketInventory>(hasInventory);
            }
            else
            {
                hasInventory = new TicketInventory
                {
                    Id = Guid.NewGuid(),
                    AppInventoryId = inventory.Id,
                    //AppTicketId = Guid.Parse(command.TicketId),
                    AdmissionPrescriptionId = prescriptionFromDb.Id,
                    DoctorsPrescription = string.IsNullOrEmpty(request.DoctorsPrescription) ? null : request.DoctorsPrescription,
                    PrescribedQuantity = string.IsNullOrEmpty(request.PrescribedQuantity) ? null : request.PrescribedQuantity,
                    Times = request.Times,
                    Dosage = request.Dosage,
                    Frequency = request.Frequency,
                    Duration = request.Duration
                };

                await iDBRepository.AddAsync<TicketInventory>(hasInventory);
            }

            ticketsInventoriesUpdated.Add(hasInventory.Id.Value);
        }
    }
}
