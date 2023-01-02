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

namespace Application.Command.AddPharmacyTicketInventory
{
    public class AddPharmacyTicketInventoryCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
        public ICollection<AddPharmacyTicketRequest>? TicketInventories { get; set; }
    }

    public class AddPharmacyTicketInventoryHandler : IRequestHandler<AddPharmacyTicketInventoryCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public AddPharmacyTicketInventoryHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<Unit> Handle(AddPharmacyTicketInventoryCommand request, CancellationToken cancellationToken)
        {
            await PharmacyTicketInventoryHelper.AddOrUpdateExistingPharmacyTickets(request, iTicketRepository, iInventoryRepository, iDBRepository);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
