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

namespace Application.Command.AdmissionEntities.DeleteAdmissionPrescription
{
    public class DeleteAdmissionPrescriptionCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? AdmissionPrescriptionId { get; set; }
    }

    public class DeleteAdmissionPrescriptionHandler : IRequestHandler<DeleteAdmissionPrescriptionCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IDBRepository iDBRepository;

        public DeleteAdmissionPrescriptionHandler(ITicketRepository iTicketRepository, IDBRepository iDBRepository)
        {
            this.iTicketRepository = iTicketRepository;
            this.iDBRepository = iDBRepository;
        }
        public async Task<Unit> Handle(DeleteAdmissionPrescriptionCommand request, CancellationToken cancellationToken)
        {
            var prescription = await iTicketRepository.TicketPrescription().FirstOrDefaultAsync(x => x.Id.ToString() == request.AdmissionPrescriptionId);

            if (prescription == null)
            {
                throw new CustomMessageException("Prescription to delete not found");
            }

            if (prescription.AppTicketStatus != Models.Enums.AppTicketStatus.ongoing)
            {
                throw new CustomMessageException("Ticket must be in the not concluded state to be deleted");
            }

            iDBRepository.Remove<AdmissionPrescription>(prescription);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
