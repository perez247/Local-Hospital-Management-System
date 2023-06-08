using Application.Annotations;
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

namespace Application.Command.TicketEntities.ConcludeLabRadTicket
{
    public class ConcludeLabRadTicketCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }

        public ICollection<ConcludeLabRadTicketRequest>? ConcludeTicketRequest { get; set; }
    }

    public class ConcludeLabRadTicketHandler : IRequestHandler<ConcludeLabRadTicketCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public ConcludeLabRadTicketHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }

        public async Task<Unit> Handle(ConcludeLabRadTicketCommand request, CancellationToken cancellationToken)
        {
            AppTicket? ticketFromDb = await TicketHelper.BasicConclusion(request.TicketId, request.ConcludeTicketRequest, iTicketRepository, iDBRepository, false);

            foreach (var genericTicketInventory in request.ConcludeTicketRequest)
            {
                var ticketInventory = ticketFromDb.TicketInventories.FirstOrDefault(x => x.Id.ToString() == genericTicketInventory.InventoryId);

                ticketInventory.LabRadiologyTestResult = genericTicketInventory.LabRadiologyTestResult;

                ticketInventory.ItemsUsed = genericTicketInventory.ItemsUsed;

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
