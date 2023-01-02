﻿using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.ConcludeLabTicketInventory
{
    public class ConcludeLabTicketInventoryCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
        public ICollection<ConcludeLabTicketInventoryRequest>? ConcludeLabTicketInventoryRequest { get; set; }
    }

    public class ConcludeLabTicketInventoryHandler : IRequestHandler<ConcludeLabTicketInventoryCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public ConcludeLabTicketInventoryHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<Unit> Handle(ConcludeLabTicketInventoryCommand request, CancellationToken cancellationToken)
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                              .Include(x => x.AppCost)
                              .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustHaveBeenSentToFinance();

            var appCost = ticketFromDb.AppCost;
            if (appCost.PaymentStatus == PaymentStatus.pending)
            {
                throw new CustomMessageException("Payment status should not be pending");
            }

            request.ConcludeLabTicketInventoryRequest = request.ConcludeLabTicketInventoryRequest.DistinctBy(x => x.InventoryId).ToList();


            var inventoryIds = request.ConcludeLabTicketInventoryRequest.Select(y => y.InventoryId);

            var inventories = await iInventoryRepository.AppInventories()
                                                        .Where(x => inventoryIds.Contains(x.Id.ToString()))
                                                        .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                                           .Where(x => x.AppTicketId == ticketFromDb.Id)
                                                           .ToListAsync();

            foreach (var genericTicketInventory in request.ConcludeLabTicketInventoryRequest)
            {
                var inventory = inventories.FirstOrDefault(x => x.Id.ToString() == genericTicketInventory.InventoryId);

                if (inventory == null)
                {
                    throw new CustomMessageException("Item not recorded in the inventory");
                }

                var ticketInventory = ticketInventories.FirstOrDefault(x => x.AppTicketId == ticketFromDb.Id);

                if (ticketInventory == null)
                {
                    throw new CustomMessageException("Item not found for this ticket");
                }

                ticketInventory.ConcludedDate = genericTicketInventory.ConcludedDate;
                ticketInventory.LabRadiologyTestResult = genericTicketInventory.LabRadiologyTestResult;
                ticketInventory.AppTicketStatus = genericTicketInventory.AppTicketStatus.ParseEnum<AppTicketStatus>();
                ticketInventory.Proof = genericTicketInventory.Proof;

                iDBRepository.Update<TicketInventory>(ticketInventory);

            }

            await iDBRepository.Complete();

            var confimTickts = await iTicketRepository.TicketInventory()
                              .Where(x => x.AppTicketId.ToString() == request.TicketId)
                              .ToListAsync();

            var totalConcluded = confimTickts.Where(x => x.AppTicketStatus == AppTicketStatus.concluded || x.AppTicketStatus == AppTicketStatus.canceled);

            if (totalConcluded.Count() == confimTickts.Count)
            {
                if (ticketFromDb.AppTicketStatus != AppTicketStatus.concluded)
                {
                    ticketFromDb.AppTicketStatus = AppTicketStatus.concluded;
                    iDBRepository.Update<AppTicket>(ticketFromDb);
                    await iDBRepository.Complete();
                }
            }

            return Unit.Value;
        }
    }
}
