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

namespace Application.Command.TicketEntities.ConcludeSurgeryTicket
{
    public class ConcludeSurgeryTicketCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
        public ICollection<ConcludeSurgeryTicketRequest>? ConcludeTicketRequest { get; set; }
    }

    public class ConcludeSurgeryTicketHandler : IRequestHandler<ConcludeSurgeryTicketCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public ConcludeSurgeryTicketHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<Unit> Handle(ConcludeSurgeryTicketCommand request, CancellationToken cancellationToken)
        {
            AppTicket? ticketFromDb = await TicketHelper.BasicConclusion(request.TicketId, request.ConcludeTicketRequest, iTicketRepository, iDBRepository);

            foreach (var genericTicketInventory in request.ConcludeTicketRequest)
            {
                var ticketInventory = ticketFromDb.TicketInventories.FirstOrDefault(x => x.Id.ToString() == genericTicketInventory.InventoryId);

                ticketInventory.SurgeryTestResult = genericTicketInventory.SurgeryTestResult;

                ticketInventory.ItemsUsed = genericTicketInventory.ItemsUsed;

                if (ticketInventory.SurgeryTicketPersonnels.Count <= 0)
                {
                    throw new CustomMessageException("No staff added to this surgery ticket");
                }

                if (!ticketInventory.SurgeryDate.HasValue)
                {
                    throw new CustomMessageException("Date of surgery not given");
                }

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
                    iDBRepository.Update(ticketFromDb);
                    await iDBRepository.Complete();
                }
            }

            return Unit.Value;
        }
    }
}
