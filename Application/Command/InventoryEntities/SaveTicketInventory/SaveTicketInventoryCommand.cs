using Application.Annotations;
using Application.Command.TicketEntities.UpdateSurgeryTicket;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Responses;
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

namespace Application.Command.InventoryEntities.SaveTicketInventory
{
    public class AppInventoryRequest
    {
        public BaseResponse? Base { get; set; }
    }
    public class SaveTicketInventoryCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketInventoryId { get; set; }
        public string? AppTicketStatus { get; set; }
        public int? Times { get; set; }
        public int? Dosage { get; set; }
        public int? Duration { get; set; }
        public string? Frequency { get; set; }
        public string? DoctorsPrescription { get; set; }
        public string? PrescribedQuantity { get; set; }
        public string? DepartmentDescription { get; set; }
        public string? AdditionalNote { get; set; }
        public string? labRadiologyTestResult { get; set; }
        public string? SurgeryTestResult { get; set; }
        public string? StaffObservation { get; set; }
        public AppInventoryRequest? Inventory { get; set; }
        public DateTime? AdmissionStartDate { get; set; }
        public DateTime? AdmissionEndDate { get; set; }
        public ICollection<UpdateSurgeryTicketPersonnel>? SurgeryTicketPersonnels { get; set; }
        public ICollection<string>? Proof { get; set; }
        public ICollection<SaveTicketInventoryDebtor>? Debtors { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? ConcludedPrice { get; set; }
    }

    public class SaveTicketInventoryHandler : IRequestHandler<SaveTicketInventoryCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public SaveTicketInventoryHandler(ITicketRepository iTicketRepository, IInventoryRepository iInventoryRepository, IDBRepository iDBRepository)
        {
            this.iTicketRepository = iTicketRepository;
            this.iInventoryRepository = iInventoryRepository;
            this.iDBRepository = iDBRepository;
        }
        public async Task<Unit> Handle(SaveTicketInventoryCommand request, CancellationToken cancellationToken)
        {
            var ticketInventory = await iTicketRepository.TicketInventory()
                                                         .Include(x => x.AppTicket)
                                                         .Include(x => x.AppInventory)
                                                         .Include(x => x.SurgeryTicketPersonnels)
                                                         .Include(x => x.TicketInventoryDebtors)
                                                         .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketInventoryId);

            if (ticketInventory == null)
            {
                throw new CustomMessageException("Failed to get ticket inventory");
            }

            if (ticketInventory.AppTicket == null)
            {
                throw new CustomMessageException("Failed to get ticket");
            }

            ticketInventory.AppTicketStatus = request.AppTicketStatus.ParseEnum<AppTicketStatus>();
            ticketInventory.Times = request.Times;
            ticketInventory.Dosage = request.Dosage;
            ticketInventory.Duration = request.Duration;
            ticketInventory.Frequency = request.Frequency;
            ticketInventory.DoctorsPrescription = request.DoctorsPrescription;
            ticketInventory.PrescribedQuantity = request.PrescribedQuantity;
            ticketInventory.DepartmentDescription = request.DepartmentDescription;
            ticketInventory.AdditionalNote = request.AdditionalNote;
            ticketInventory.StaffObservation = request.StaffObservation;
            ticketInventory.ConcludedPrice = request.ConcludedPrice;


            #region Lab or radiology
            ticketInventory.LabRadiologyTestResult = request.labRadiologyTestResult;
            ticketInventory.Proof = request.Proof != null ? request.Proof : new List<string>();
            #endregion

            #region Admission
            ticketInventory.AdmissionStartDate = request.AdmissionStartDate;
            ticketInventory.AdmissionEndDate = request.AdmissionEndDate;

            if (ticketInventory.AppInventoryId.ToString() != request.Inventory.Base.Id)
            {
                var found = await iInventoryRepository.AppInventories().FirstOrDefaultAsync(x => x.Id.ToString() == request.Inventory.Base.Id);

                if (found == null)
                {
                    throw new CustomMessageException("Updated admission not found");
                }

                ticketInventory.AppInventoryId = Guid.Parse(request.Inventory.Base.Id);
            }

            #endregion

            if (ticketInventory.LoggedQuantity.HasValue && ticketInventory.LoggedQuantity.Value)
            {
                FinancialHelper.UpdateQuantity(ticketInventory, ticketInventory.AppInventory, Int32.Parse(request.PrescribedQuantity), request.getCurrentUserRequest().CurrentUser.Id, iDBRepository, nameof(SaveTicketInventoryCommand));
            }

            #region Surgery

            ticketInventory.SurgeryTestResult = request.SurgeryTestResult;

            if (request.SurgeryTicketPersonnels != null && request.SurgeryTicketPersonnels.Count > 0)
            {
                request.SurgeryTicketPersonnels = request.SurgeryTicketPersonnels.DistinctBy(x => x.PersonnelId).ToList();

                if (ticketInventory.SurgeryTicketPersonnels.Count > 0)
                {
                    foreach (var personnel in ticketInventory.SurgeryTicketPersonnels)
                    {
                        iDBRepository.Remove<SurgeryTicketPersonnel>(personnel);
                    }
                }

                foreach (var personnel in request.SurgeryTicketPersonnels)
                {
                    await iDBRepository.AddAsync<SurgeryTicketPersonnel>(new SurgeryTicketPersonnel
                    {
                        TicketInventoryId = ticketInventory.Id,
                        PersonnelId = Guid.Parse(personnel.PersonnelId),
                        SurgeryRole = personnel.SurgeryRole.Trim(),
                        IsHeadPersonnel = personnel.IsHeadPersonnel,
                    });
                }
            }

            #endregion

            await UpdateDebtors(request, ticketInventory);

            ticketInventory.Updated = DateTime.Now.ToUniversalTime();

            iDBRepository.Update<TicketInventory>(ticketInventory);

            await iDBRepository.Complete();

            return Unit.Value;
        }

        private async Task UpdateDebtors(SaveTicketInventoryCommand request, TicketInventory? ticketInventory)
        {
            if (request.Debtors != null && request.Debtors.Count > 0)
            {
                string id = null;
                var sum = 0.0m;
                foreach (var debtor in ticketInventory.TicketInventoryDebtors)
                {
                    iDBRepository.Remove<TicketInventoryDebtor>(debtor);
                }

                foreach (var newDebtor in request.Debtors)
                {
                    if (newDebtor.PayerId == id)
                    {
                        throw new CustomMessageException("Only one payer should be added");
                    }

                    sum += newDebtor.Amount.Value;

                    await iDBRepository.AddAsync<TicketInventoryDebtor>(new TicketInventoryDebtor
                    {
                        PayerId = Guid.Parse(newDebtor.PayerId),
                        TicketInventoryId = ticketInventory.Id,
                        Amount = newDebtor.Amount,
                        Description = newDebtor.Description,
                    });
                }

                //var totalPrice = ticketInventory.TotalPrice != null ? ticketInventory.TotalPrice : request.TotalPrice;
                var totalPrice = ticketInventory.ConcludedPrice;

                if (sum != totalPrice)
                {
                    throw new CustomMessageException($"{ticketInventory.AppInventory.Name} total price for payers must be equal to {totalPrice}");
                }
            }

            if (request.Debtors == null || request.Debtors.Count == 0)
            {
                foreach (var debtor in ticketInventory.TicketInventoryDebtors)
                {
                    iDBRepository.Remove<TicketInventoryDebtor>(debtor);
                }
            }
        }
    }
}
