using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.ConcludeInventory
{
    public class ConcludeInventoryCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketInventoryId { get; set; }
    }

    public class ConcludeInventorHandler : IRequestHandler<ConcludeInventoryCommand, Unit>
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly IDBRepository iDBRepository;

        public ConcludeInventorHandler(IInventoryRepository InventoryRepository, IDBRepository IDBRepository)
        {
            inventoryRepository = InventoryRepository;
            iDBRepository = IDBRepository;
        }

        public async Task<Unit> Handle(ConcludeInventoryCommand request, CancellationToken cancellationToken)
        {
            var ticketInventory = await inventoryRepository.TicketInventories()
                                                           .Include(x => x.AdmissionPrescription)
                                                           .Include(x => x.SurgeryTicketPersonnels)
                                                           .Include(x => x.AppInventory)
                                                           .Include(x => x.AppTicket)
                                                            .ThenInclude(x => x.TicketInventories)
                                                           .Include(x => x.AppTicket)
                                                            .ThenInclude(x => x.AppCost)
                                                           .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketInventoryId);

            if (ticketInventory == null)
            {
                throw new CustomMessageException("Ticket Inventory not found");
            }

            if (ticketInventory.ConcludedDate.HasValue)
            {
                throw new CustomMessageException($"{ticketInventory.AppInventory.Name} Has been concluded");
            }

            if (ticketInventory.AppTicket.AppCost == null && ticketInventory.AdmissionPrescription == null)
            {
                throw new CustomMessageException("Client must be billed first, except this is for admission patients");
            }

            if (ticketInventory.AppInventory.AppInventoryType == Models.Enums.AppInventoryType.surgery)
            {
                if (string.IsNullOrEmpty(ticketInventory.SurgeryTestResult))
                {
                    throw new CustomMessageException("Result for surgery is required before conclusion");
                }

                if (ticketInventory.SurgeryTicketPersonnels == null || ticketInventory.SurgeryTicketPersonnels.Count <= 0)
                {
                    throw new CustomMessageException("At least one surgery personnel is required before conclusion");
                }
            }

            if (ticketInventory.AppInventory.AppInventoryType == Models.Enums.AppInventoryType.lab || ticketInventory.AppInventory.AppInventoryType == Models.Enums.AppInventoryType.radiology)
            {
                if (string.IsNullOrEmpty(ticketInventory.LabRadiologyTestResult))
                {
                    throw new CustomMessageException("Result for lab/radiology is required before conclusion");
                }
            }

            ticketInventory.ConcludedDate = DateTime.Now.ToUniversalTime();

            iDBRepository.Update<TicketInventory>(ticketInventory);

            var ticketInventories = ticketInventory.AppTicket.TicketInventories;

            var concludedInventories = ticketInventories.Where(x => x.ConcludedDate.HasValue);

            if (concludedInventories.Count() == ticketInventories.Count)
            {
                ticketInventory.AppTicket.AppTicketStatus = Models.Enums.AppTicketStatus.concluded;
                iDBRepository.Update<AppTicket>(ticketInventory.AppTicket);
            }

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
