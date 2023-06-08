﻿using Application.Annotations;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Requests;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.SendPharmacyTicketToFinance
{
    public class SendPharmacyTicketToFinanceCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
        public ICollection<SendTicketToFinanceRequest>? TicketInventories { get; set; }
    }

    public class SendTicketToFinanceHandler : IRequestHandler<SendPharmacyTicketToFinanceCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public SendTicketToFinanceHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iInventoryRepository = IInventoryRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(SendPharmacyTicketToFinanceCommand request, CancellationToken cancellationToken)
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                                      .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustHvaeBeenSentToDepartment();
            ticketFromDb.CancelIfSentToDepartmentAndFinance();

            request.TicketInventories = request.TicketInventories
                                               .DistinctBy(x => x.InventoryId).ToList();

            var inventoryIds = request.TicketInventories.Select(y => y.InventoryId);

            await TicketHelper.VerifyInventories(request.TicketInventories, ticketFromDb, inventoryIds, iTicketRepository, iInventoryRepository, iDBRepository, PharmacyValidation);

            ticketFromDb.SentToFinance = true;
            iDBRepository.Update(ticketFromDb);

            await iDBRepository.Complete();

            return Unit.Value;
        }

        private static object PharmacyValidation(
            SendTicketToFinanceRequest itemTicket,
            AppInventory inventory
            )
        {
            if (inventory.Quantity < itemTicket.PrescribedQuantity)
            {
                throw new CustomMessageException($"{inventory.Name} does not have enough available quantity");
            }
            return new object();
        }
        
    }
}