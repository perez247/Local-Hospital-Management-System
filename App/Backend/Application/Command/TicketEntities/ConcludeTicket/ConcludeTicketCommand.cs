using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.ConcludeTicket
{
    public class ConcludeTicketCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
    }

    public class ConcludeTicketHandler : IRequestHandler<ConcludeTicketCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IDBRepository iDBRepository;

        public ConcludeTicketHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
        }

        public async Task<Unit> Handle(ConcludeTicketCommand request, CancellationToken cancellationToken)
        {
            AppTicket? ticketFromDb = await TicketHelper.BasicConclusion2(request.TicketId, iTicketRepository, iDBRepository);

            foreach (var item in ticketFromDb.TicketInventories.Where(x => x.AppTicketStatus == Models.Enums.AppTicketStatus.ongoing))
            {
                InventoryHelper.ValidateInventoryConclusion(item);
            }

            await iDBRepository.Complete();

            return Unit.Value;
        }

    }
}
