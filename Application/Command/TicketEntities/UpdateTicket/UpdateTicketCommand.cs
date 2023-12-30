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

namespace Application.Command.TicketEntities.UpdateTicket
{
    public class UpdateTicketCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
        public string? OverallDescription { get; set; }
        public bool? Sent { get; set; }
        public bool? SentToFinance { get; set; }
        public string? AppTicketStatus { get; set; }
    }

    public class UpdateTicketHandler : IRequestHandler<UpdateTicketCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateTicketHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iInventoryRepository = IInventoryRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                                      .Include(x => x.TicketInventories)
                                                        .ThenInclude(a => a.AppInventory)
                                                      .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.CancelIfSentToDepartmentAndFinance();

            ticketFromDb.OverallDescription = string.IsNullOrEmpty(request.OverallDescription) ? null : request.OverallDescription.Trim();
            ticketFromDb.Sent = request.Sent;
            ticketFromDb.SentToFinance = request.SentToFinance;


            iDBRepository.Update(ticketFromDb);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
