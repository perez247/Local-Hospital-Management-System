using Application.Annotations;
using Application.Command.SaveSurgeryTicketInventory;
using Application.Exceptions;
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

namespace Application.Command.UpdateSurgeryTicketInventory
{
    public class UpdateSurgeryTicketInventoryCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
        public ICollection<UpdateSurgeryTicketInventoryRequest>? UpdateSurgeryTicketInventoryRequest { get; set; }
    }

    public class UpdateSurgeryTicketInventoryHandler : IRequestHandler<UpdateSurgeryTicketInventoryCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateSurgeryTicketInventoryHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<Unit> Handle(UpdateSurgeryTicketInventoryCommand request, CancellationToken cancellationToken)
        {
            await SurgeryTicketInventoryHelper.UpdateSurgeryTicketList(request, iTicketRepository, iInventoryRepository, iDBRepository);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
