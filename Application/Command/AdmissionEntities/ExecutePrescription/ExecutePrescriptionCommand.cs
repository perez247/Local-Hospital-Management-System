﻿using Application.Annotations;
using Application.Command.TicketEntities.AddPharmacyTicketInventory;
using Application.Command.TicketEntities.SaveTicketAndInventory;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using Application.Utilities.QueryHelpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
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
        public string? AppTicketStatus { get; set; }
        public string? PrescribedQuantity { get; set; }
        public string? DepartmentDescription { get; set; }
        public string? StaffObservation { get; set; }
        public string? AdditionalNote { get; set; }

        [VerifyGuidAnnotation]
        public string? StaffResponsible { get; set; }
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
                                                                .Include(x => x.AdmissionPrescription)
                                                                    .ThenInclude(x => x.AppTicket)
                                                                        .ThenInclude(x => x.Appointment)
                                                                .Include(x => x.AppTicket)
                                                                    .ThenInclude(x => x.AppCost)
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

            if (ticketPrecriptionFromDb.AdmissionPrescription.AppTicket.AppCost != null)
            {
                throw new CustomMessageException("This patient has been billed, kindly contact a doctor to create a new ticket");
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
                        PrescribedQuantity = "0",
                        AppInventoryType = ticketPrecriptionFromDb.AppInventory.AppInventoryType.ToString(),
                        Times = ticketPrecriptionFromDb.Times,
                        Dosage = ticketPrecriptionFromDb.Dosage,
                        Frequency = ticketPrecriptionFromDb.Frequency,
                        Duration = ticketPrecriptionFromDb.Duration,
                        StaffObservation = request.StaffObservation,
                    }
                },
            };

            newRequest.SetCurrentUserRequest(request.getCurrentUserRequest());

            var ticketTobeSaved = await TicketHelper.AddOrUpdateExistingTickets(newRequest, iTicketRepository, iInventoryRepository, iDBRepository, iAppointmentRepository, true);

            await iDBRepository.Complete();

            var newTickets = ticketTobeSaved.TicketInventories.ToList();

            foreach (var item in newTickets)
            {
                var inventoryItems = await iInventoryRepository.AppInventoryItems()
                                                               .FirstOrDefaultAsync(x => x.AppInventoryId == item.AppInventoryId && x.CompanyId == ticketPrecriptionFromDb.AdmissionPrescription.AppTicket.Appointment.CompanyId);

                item.TimeGiven = request.TimeGiven.Value.ToUniversalTime();
                item.AdmissionPrescriptionId = ticketPrecriptionFromDb.AdmissionPrescriptionId;
                item.AppTicketId = ticketPrecriptionFromDb.AdmissionPrescription.AppTicketId;
                item.PrescribedQuantity = request.PrescribedQuantity;
                item.AppTicketStatus = request.AppTicketStatus.ParseEnum<AppTicketStatus>();
                item.AdditionalNote = request.AdditionalNote;

                //if (item.AppInventory.AppInventoryType == AppInventoryType.pharmacy)
                //{
                //    item.ConcludedDate = DateTime.Now.ToUniversalTime();
                //}

                item.TotalPrice = decimal.Parse(item.PrescribedQuantity) * inventoryItems.PricePerItem;
                item.ConcludedPrice = item.TotalPrice;
                item.DepartmentDescription = request.DepartmentDescription;
                item.Updated = DateTime.Now.ToUniversalTime();
                item.LoggedQuantity = true;
                item.StaffResponsible = request.StaffResponsible == Guid.Empty.ToString() ? request.getCurrentUserRequest().CurrentUser.Id : Guid.Parse(request.StaffResponsible);

                FinancialHelper.UpdateQuantity(item, item.AppInventory, Int32.Parse(request.PrescribedQuantity), request.getCurrentUserRequest().CurrentUser.Id, iDBRepository, nameof(ExecutePrescriptionCommand));
                iDBRepository.Update<TicketInventory>(item);
            }

            await iDBRepository.Complete();

            return Unit.Value;

        }
    }
}
