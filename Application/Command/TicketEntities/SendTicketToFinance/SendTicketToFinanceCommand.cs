using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.SendTicketToFinance
{
    public class SendTicketToFinanceCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
    }

    public class SendTicketToFinanceHandler : IRequestHandler<SendTicketToFinanceCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public SendTicketToFinanceHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iInventoryRepository = IInventoryRepository;
            iDBRepository = IDBRepository;

        }
        public async Task<Unit> Handle(SendTicketToFinanceCommand request, CancellationToken cancellationToken)
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                          .Include(x => x.TicketInventories)
                                          .ThenInclude(x => x.AppInventory)
                                          .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustHvaeBeenSentToDepartment();
            ticketFromDb.CancelIfSentToDepartmentAndFinance();

            foreach (var item in ticketFromDb.TicketInventories)
            {
                if (item.AppInventory.Quantity < Int32.Parse(item.PrescribedQuantity))
                {
                    throw new CustomMessageException($"{item.AppInventory.Name} does not have enough available quantity");
                }
            }

            ticketFromDb.SentToFinance = true;
            iDBRepository.Update(ticketFromDb);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
