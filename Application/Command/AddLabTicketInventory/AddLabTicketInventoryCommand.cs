using Application.Annotations;
using Application.Command.AddPharmacyTicketInventory;
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

namespace Application.Command.AddLabTicketInventory
{
    public class AddLabTicketInventoryCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
        public ICollection<AddLabTicketInventoryRequest>? AddLabTicketInventoryRequest { get; set; }
    }

    public class AddLabTicketInventoryHandler : IRequestHandler<AddLabTicketInventoryCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public AddLabTicketInventoryHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }

        public async Task<Unit> Handle(AddLabTicketInventoryCommand request, CancellationToken cancellationToken)
        {
            await LabTicketInventoryHelper.AddNewOrExistingLabTickets(request, iTicketRepository, iInventoryRepository, iDBRepository);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
