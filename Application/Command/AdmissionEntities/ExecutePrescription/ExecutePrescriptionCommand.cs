using Application.Annotations;
using Application.Command.TicketEntities.AddPharmacyTicketInventory;
using Application.Command.TicketEntities.SaveTicketAndInventory;
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

namespace Application.Command.AdmissionEntities.ExecutePrescription
{
    public class ExecutePrescriptionCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketInventoryId { get; set; }
        public DateTime? TimeGiven { get; set; }
    }

    public class ExecutePrescriptionHandler : IRequestHandler<ExecutePrescriptionCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IDBRepository iDBRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IAppointmentRepository iAppointmentRepository;

        public ExecutePrescriptionHandler(ITicketRepository iTicketRepository, IDBRepository iDBRepository, IInventoryRepository iInventoryRepository, IAppointmentRepository iAppointmentRepository)
        {
            this.iTicketRepository = iTicketRepository;
            this.iDBRepository = iDBRepository;
            this.iInventoryRepository = iInventoryRepository;
            this.iAppointmentRepository = iAppointmentRepository;
        }

        public async Task<Unit> Handle(ExecutePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var ticketPrecriptionFromDb = await iTicketRepository.TicketInventory()
                                                                .Include(x => x.AdmissionPrescription)
                                                                    .ThenInclude(x => x.AppTicket)
                                                                .Include(x => x.AppTicket)
                                                                .Include(x => x.AppInventory)
                                                                .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketInventoryId);

            if (ticketPrecriptionFromDb == null)
            {
                throw new CustomMessageException("Ticket inventory not found");
            }

            if (ticketPrecriptionFromDb.AdmissionPrescription == null)
            {
                throw new CustomMessageException("Prescription not found");
            }

            if (ticketPrecriptionFromDb.AppTicket != null)
            {
                throw new CustomMessageException("Ticket Inventory already has an App Ticket");
            }

            var newRequest = new SaveTicketAndInventoryCommand
            {
                TicketId = ticketPrecriptionFromDb.AdmissionPrescription.AppTicketId.ToString(),
                OverallDescription = ticketPrecriptionFromDb.AdmissionPrescription.OverallDescription,
                AppInventoryType = ticketPrecriptionFromDb.AdmissionPrescription.AppInventoryType.ToString(),
                AppointmentId = ticketPrecriptionFromDb.AdmissionPrescription.AppTicket.AppointmentId.ToString(),
                TicketInventories = new List<SaveTicketAndInventoryRequest>
                {
                    new SaveTicketAndInventoryRequest { 
                        InventoryId =  ticketPrecriptionFromDb.AppInventory.Id.ToString(),
                        DoctorsPrescription = ticketPrecriptionFromDb.DoctorsPrescription,
                        PrescribedQuantity = ticketPrecriptionFromDb.Dosage.ToString(),
                        AppInventoryType = ticketPrecriptionFromDb.AppInventory.AppInventoryType.ToString(),
                        Times = ticketPrecriptionFromDb.Times,
                        Dosage = ticketPrecriptionFromDb.Dosage,
                        Frequency = ticketPrecriptionFromDb.Frequency,
                    }
                },
            };

            newRequest.SetCurrentUserRequest(request.getCurrentUserRequest());

            var ticketTobeSaved = await TicketHelper.AddOrUpdateExistingTickets(newRequest, iTicketRepository, iInventoryRepository, iDBRepository, iAppointmentRepository, true);

            await iDBRepository.Complete();

            var newTickets = ticketTobeSaved.TicketInventories.ToList();

            foreach (var item in newTickets)
            {
                item.TimeGiven = request.TimeGiven.Value.ToUniversalTime();
                item.AdmissionPrescriptionId = ticketPrecriptionFromDb.AdmissionPrescriptionId;
                item.AppTicketId = ticketPrecriptionFromDb.AdmissionPrescription.AppTicketId;
                iDBRepository.Update<TicketInventory>(item);
            }

            await iDBRepository.Complete();

            return Unit.Value;

        }
    }
}
