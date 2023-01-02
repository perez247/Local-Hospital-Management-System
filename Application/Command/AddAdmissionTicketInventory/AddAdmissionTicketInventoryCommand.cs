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

namespace Application.Command.AddAdmissionTicketInventory
{
    public class AddAdmissionTicketInventoryCommand : TokenCredentials, IRequest<Unit>
    {

        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }

        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? CurrentPrice { get; set; }
    }

    public class AddAdmissionTicketInventoryHandler : IRequestHandler<AddAdmissionTicketInventoryCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public AddAdmissionTicketInventoryHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<Unit> Handle(AddAdmissionTicketInventoryCommand request, CancellationToken cancellationToken)
        {
            await AdmissionTickInventoryHelper.AddAdmissionTicket(request, iTicketRepository, iInventoryRepository, iDBRepository);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
