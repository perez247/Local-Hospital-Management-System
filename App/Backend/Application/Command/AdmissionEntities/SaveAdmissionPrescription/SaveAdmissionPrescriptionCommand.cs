using Application.Annotations;
using Application.Command.TicketEntities.SaveTicketAndInventory;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AdmissionEntities.SaveAdmissionPrescription
{
    public class SaveAdmissionPrescriptionCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? PrescriptionId { get; set; }

        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
        public string? AppInventoryType { get; set; }
        public string? OverallDescription { get; set; }
        public string? AppTicketStatus { get; set; }
        public ICollection<SaveAdmissionPrescriptionRequest>? TicketInventories { get; set; }
    }

    public class SaveAdmissionPrescriptionHandler : IRequestHandler<SaveAdmissionPrescriptionCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public SaveAdmissionPrescriptionHandler(ITicketRepository iTicketRepository, IInventoryRepository iInventoryRepository, IDBRepository iDBRepository)
        {
            this.iTicketRepository = iTicketRepository;
            this.iInventoryRepository = iInventoryRepository;
            this.iDBRepository = iDBRepository;
        }

        public async Task<Unit> Handle(SaveAdmissionPrescriptionCommand request, CancellationToken cancellationToken)
        {
            await PrescriptionHelper.AddOrUpdateExistingAdmissionPrescription(request, iTicketRepository, iInventoryRepository, iDBRepository);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
