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

namespace Application.Command.CreateTicket
{
    public class CreateTicketCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? AppointmentId { get; set; }
        public string? AppInventoryType { get; set; }
    }

    public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, Unit>
    {

        private readonly IDBRepository iDBRepository;
        private readonly IAppointmentRepository iAppointmentRepository;

        public CreateTicketHandler(IDBRepository iDBRepository, IAppointmentRepository iAppointmentRepository)
        {
            this.iDBRepository = iDBRepository;
            this.iAppointmentRepository = iAppointmentRepository;
        }

        public async Task<Unit> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            AppTicket newTicket = await TicketHelper.CreateNewTicket(request, iAppointmentRepository);

            await iDBRepository.AddAsync<AppTicket>(newTicket);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
