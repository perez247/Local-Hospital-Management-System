using Application.Annotations;
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

namespace Application.Command.TicketEntities.DeleteTicket
{
    public class DeleteTicketCommand: TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
    }

    public class DeleteTicketHandler : IRequestHandler<DeleteTicketCommand, Unit>
    {
        public ITicketRepository? _ticketRepository { get; set; }
        public IDBRepository? _dBRepository { get; set; }

        public DeleteTicketHandler(ITicketRepository? ticketRepository, IDBRepository? dBRepository)
        {
            _ticketRepository = ticketRepository;
            _dBRepository = dBRepository;
        }

        public async Task<Unit> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            var ticketToDelete = await _ticketRepository.AppTickets()
                                                        .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketToDelete.MustNotHaveBeenSentToDepartment();

            _dBRepository.Remove<AppTicket>(ticketToDelete);

            await _dBRepository.Complete();

            return Unit.Value;
        }
    }
}
