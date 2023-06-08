using Application.Annotations;
using Application.Command.TicketEntities.AddPharmacyTicketInventory;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.SaveTicketAndInventory
{
    public class SaveTicketAndInventoryCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }

        [VerifyGuidAnnotation]
        public string? AppointmentId { get; set; }
        public string? AppInventoryType { get; set; }
        public string? OverallDescription { get; set; }
        public ICollection<SaveTicketAndInventoryRequest>? TicketInventories { get; set; }
    }

    public class SaveTicketAndInventoryHandler : IRequestHandler<SaveTicketAndInventoryCommand, Unit>
    {

        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public SaveTicketAndInventoryHandler(ITicketRepository iTicketRepository, IInventoryRepository iInventoryRepository, IDBRepository iDBRepository)
        {
            this.iTicketRepository = iTicketRepository;
            this.iInventoryRepository = iInventoryRepository;
            this.iDBRepository = iDBRepository;
        }
        public async Task<Unit> Handle(SaveTicketAndInventoryCommand request, CancellationToken cancellationToken)
        {
            await TicketHelper.AddOrUpdateExistingTickets(request, iTicketRepository, iInventoryRepository, iDBRepository);
            
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
