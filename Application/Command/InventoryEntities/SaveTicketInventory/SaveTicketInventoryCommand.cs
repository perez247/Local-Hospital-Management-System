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
        public AppInventoryRequest? Inventory { get; set; }
        public DateTime? AdmissionStartDate { get; set; }
        public DateTime? AdmissionEndDate { get; set; }
        public ICollection<UpdateSurgeryTicketPersonnel>? SurgeryTicketPersonnels { get; set; }
        public ICollection<string>? Proof { get; set; }
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
                var oldQuantity = ticketInventory.AppInventory.Quantity;

                FinancialHelper.UpdateQuantity(ticketInventory, ticketInventory.AppInventory, Int32.Parse(request.PrescribedQuantity));

                if (oldQuantity != ticketInventory.AppInventory.Quantity)
                {
                    iDBRepository.Update<AppInventory>(ticketInventory.AppInventory);
                }
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

            ticketInventory.Updated = DateTime.Now.ToUniversalTime();

            iDBRepository.Update<TicketInventory>(ticketInventory);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
