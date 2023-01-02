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

namespace Application.Command.UpdateSurgerySummary
{
    public class UpdateSurgerySummaryCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? SurgeryTicketInventoryId { get; set; }
        public string? Summary { get; set; }
    }

    public class UpdateSurgerySummaryHandler : IRequestHandler<UpdateSurgerySummaryCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateSurgerySummaryHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<Unit> Handle(UpdateSurgerySummaryCommand request, CancellationToken cancellationToken)
        {

            var currentUserId = request.getCurrentUserRequest().CurrentUser?.Id;

            var surgeryTicketInventory = await iTicketRepository.TicketInventory()
                                                                .Include(x => x.AppTicket)
                                                                .Include(x => x.SurgeryTicketPersonnels.Where(x => x.PersonnelId == currentUserId))
                                                                .FirstOrDefaultAsync(x => x.Id.ToString() == request.SurgeryTicketInventoryId);

            if (surgeryTicketInventory == null)
            {
                throw new CustomMessageException("Surgery not found", System.Net.HttpStatusCode.NotFound);
            }

            var myPersonnel = surgeryTicketInventory.SurgeryTicketPersonnels.FirstOrDefault();

            if (myPersonnel == null)
            {
                throw new CustomMessageException("You are not a personnel in this surgery");
            }

            var ticket = surgeryTicketInventory.AppTicket;

            if (ticket == null)
            {
                throw new CustomMessageException("Surgery Ticket not found", System.Net.HttpStatusCode.NotFound);
            }

            ticket.MustHvaeBeenSentToDepartment();
            ticket.MustHaveBeenSentToFinance();

            myPersonnel.SummaryOfSurgery = request.Summary;
            iDBRepository.Update<SurgeryTicketPersonnel>(myPersonnel);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
