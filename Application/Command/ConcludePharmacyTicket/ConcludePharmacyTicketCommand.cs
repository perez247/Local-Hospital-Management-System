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

namespace Application.Command.ConcludePharmacyTicket
{
    public class ConcludePharmacyTicketCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
        public ICollection<ConcludePharmacyTicketRequest>? ConcludePharmacyTicketRequest { get; set; }
    }

    public class ConcludePharmacyTicketHandler : IRequestHandler<ConcludePharmacyTicketCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public ConcludePharmacyTicketHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<Unit> Handle(ConcludePharmacyTicketCommand request, CancellationToken cancellationToken)
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                              .Include(x => x.TicketInventories)
                                .ThenInclude(x => x.AppInventory)
                              .Include(x => x.AppCost)
                              .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustHaveBeenSentToFinance();

            var appCost = ticketFromDb.AppCost;
            if (appCost.PaymentStatus == PaymentStatus.pending)
            {
                throw new CustomMessageException("Payment status should not be pending");
            }

            request.ConcludePharmacyTicketRequest = request.ConcludePharmacyTicketRequest.DistinctBy(x => x.InventoryId).ToList();


            var ticketInventories = ticketFromDb.TicketInventories;

            foreach (var genericTicketInventory in request.ConcludePharmacyTicketRequest)
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
