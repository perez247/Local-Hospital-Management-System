using Application.Annotations;
using Application.Command.TicketEntities.ConcludeLabRadTicket;
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
using Application.Exceptions;

namespace Application.Command.TicketEntities.ConcludeAdmissionTicket
{
    public class ConcludeAdmissionTicketCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }

        public ICollection<ConcludeAdmissionTicketRequest>? ConcludeTicketRequest { get; set; }
        
    }

    public class ConcludeAdmissionTicketHandler : IRequestHandler<ConcludeAdmissionTicketCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public ConcludeAdmissionTicketHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }

        public async Task<Unit> Handle(ConcludeAdmissionTicketCommand request, CancellationToken cancellationToken)
        {
            AppTicket? ticketFromDb = await TicketHelper.BasicConclusion(request.TicketId, request.ConcludeTicketRequest, iTicketRepository, iDBRepository, false);

            ticketFromDb.AppCost.ApprovedPrice = request.ConcludeTicketRequest.Sum(x => x.TotalPrice);
            iDBRepository.Update<AppCost>(ticketFromDb.AppCost);

            foreach (var genericTicketInventory in request.ConcludeTicketRequest)
            {
                var ticketInventory = ticketFromDb.TicketInventories.FirstOrDefault(x => x.Id.ToString() == genericTicketInventory.InventoryId);

                var totalDays = genericTicketInventory.AdmissionEndDate - ticketInventory.AdmissionStartDate;

                if (totalDays.Value.TotalDays <= 0) 
                {
                    throw new CustomMessageException("Duration must be greater than zero");
                }

                ticketInventory.AdmissionEndDate = genericTicketInventory.AdmissionEndDate;


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
