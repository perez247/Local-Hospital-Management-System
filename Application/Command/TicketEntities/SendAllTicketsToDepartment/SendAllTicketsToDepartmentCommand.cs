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

namespace Application.Command.TicketEntities.SendAllTicketsToDepartment
{
    public class SendAllTicketsToDepartmentCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? AppointmentId { get; set; }
    }

    public class SendAllTicketsToDepartmentHandler : IRequestHandler<SendAllTicketsToDepartmentCommand, Unit>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IDBRepository _iDBRepository;

        public SendAllTicketsToDepartmentHandler(ITicketRepository ticketRepository, IDBRepository iDBRepository)
        {
            _ticketRepository = ticketRepository;
            _iDBRepository = iDBRepository;
        }

        public async Task<Unit> Handle(SendAllTicketsToDepartmentCommand request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.AppTickets()
                                .Where(x => x.AppointmentId.ToString() == request.AppointmentId)
                                .ToListAsync();

            if (tickets.Count == 0)
            {
                throw new CustomMessageException("No tickets found for this appointment");
            }

            foreach (var ticket in tickets)
            {
                ticket.Sent = true;
                _iDBRepository.Update(ticket);
            }

            await _iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
