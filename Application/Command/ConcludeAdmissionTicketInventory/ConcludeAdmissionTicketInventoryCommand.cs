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

namespace Application.Command.ConcludeAdmissionTicketInventory
{
    public class ConcludeAdmissionTicketInventoryCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }

        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public DateTime? ConcludedDate { get; set; }
        public string? AppTicketStatus { get; set; }
        public ICollection<string>? Proof { get; set; }
        public DateTime? AdmissionEndDate { get; set; }
    }

    public class ConcludeAdmissionTicketInventoryHandler : IRequestHandler<ConcludeAdmissionTicketInventoryCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public ConcludeAdmissionTicketInventoryHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<Unit> Handle(ConcludeAdmissionTicketInventoryCommand request, CancellationToken cancellationToken)
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                          .Include(x => x.TicketInventories.Where(y => y.Id.ToString() == request.InventoryId))
                                          .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustNotHaveBeenSentToDepartment();

            var lastAdmission = ticketFromDb.TicketInventories.FirstOrDefault();

            if (lastAdmission == null)
            {
                throw new CustomMessageException("Admission ticket inventory not found");
            }

            if (lastAdmission.AdmissionEndDate.HasValue)
            {
                throw new CustomMessageException("Last admission has been concluded");
            }

            ticketFromDb.AppTicketStatus = request.AppTicketStatus.ParseEnum<AppTicketStatus>();
            lastAdmission.ConcludedDate = request.ConcludedDate.Value;
            lastAdmission.Proof = request.Proof;
            lastAdmission.AdmissionEndDate = request.AdmissionEndDate.Value;

            if (lastAdmission.AdmissionEndDate < lastAdmission.AdmissionStartDate)
            {
                throw new CustomMessageException("Admission start date must be less than end date");
            }

            var totalDays = (lastAdmission.AdmissionEndDate - lastAdmission.AdmissionStartDate).Value.TotalDays;

            var floor = Math.Floor(totalDays);

            lastAdmission.TotalPrice = lastAdmission.CurrentPrice * (decimal)floor;

            iDBRepository.Update<AppTicket>(ticketFromDb);
            iDBRepository.Update<TicketInventory>(lastAdmission);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
