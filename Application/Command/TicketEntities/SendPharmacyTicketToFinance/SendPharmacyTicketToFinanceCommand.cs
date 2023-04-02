using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
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
        public ICollection<SendPharmacyTicketToFinanceInventory>? TicketInventories { get; set; }
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

            var inventories = await iInventoryRepository.AppInventories()
                                            .Where(x => inventoryIds.Contains(x.Id.ToString()))
                                            .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                                           .Where(x => x.AppTicketId == ticketFromDb.Id)
                                                           .ToListAsync();

            foreach (var pharmacyTicket in request.TicketInventories)
            {
                var inventory = inventories.FirstOrDefault(x => x.Id.ToString() == pharmacyTicket.InventoryId);

                if (inventory == null)
                {
                    throw new CustomMessageException("Drug not recorded in the inventory");
                }

                var pharmacyTicketInventory = ticketInventories.FirstOrDefault(x => x.Id.ToString() == pharmacyTicket.TicketInventoryId);

                if (pharmacyTicketInventory == null)
                {
                    throw new CustomMessageException("Drug not found for this ticket");
                }

                if (inventory.Quantity < pharmacyTicket.PrescribedQuantity)
                {
                    throw new CustomMessageException($"{inventory.Name} does not have enough available quantity");
                }

                pharmacyTicketInventory.PrescribedQuantity = pharmacyTicket.PrescribedQuantity.ToString();
                pharmacyTicketInventory.DepartmentDescription = pharmacyTicket.DepartmentDescription;
                pharmacyTicketInventory.AppTicketStatus = pharmacyTicket.AppTicketStatus.ParseEnum<AppTicketStatus>();

                iDBRepository.Update(pharmacyTicketInventory);
            }

            ticketFromDb.SentToFinance = true;
            iDBRepository.Update(ticketFromDb);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
