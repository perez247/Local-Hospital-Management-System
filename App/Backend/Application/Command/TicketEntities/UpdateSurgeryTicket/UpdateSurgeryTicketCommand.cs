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

namespace Application.Command.TicketEntities.UpdateSurgeryTicket
{
    public class UpdateSurgeryTicketCommand : TokenCredentials, IRequest<UpdateSurgeryTicketResponse>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }

        [VerifyGuidAnnotation]
        public string? TicketInventoryId { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public ICollection<UpdateSurgeryTicketPersonnel>? SurgeryTicketPersonnels { get; set; }
    }

    public class UpdateSurgeryTicketHandler : IRequestHandler<UpdateSurgeryTicketCommand, UpdateSurgeryTicketResponse>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateSurgeryTicketHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iInventoryRepository = IInventoryRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<UpdateSurgeryTicketResponse> Handle(UpdateSurgeryTicketCommand request, CancellationToken cancellationToken)
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                          .Include(x => x.TicketInventories.Where(x => x.Id.ToString() == request.TicketInventoryId))
                                          .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustHvaeBeenSentToDepartment();
            ticketFromDb.MustHaveBeenSentToFinance();

            var ticketInventory = ticketFromDb.TicketInventories.FirstOrDefault();

            if (ticketInventory == null)
            {
                throw new CustomMessageException("This surgery was not found");
            }

            if (ticketInventory.ConcludedDate.HasValue)
            {
                throw new CustomMessageException("This surgey has been concluded");
            }

            var save = false;

            if (ticketInventory.SurgeryTicketPersonnels.Count > 0)
            {
                foreach (var personnel in ticketInventory.SurgeryTicketPersonnels)
                {
                    iDBRepository.Remove<SurgeryTicketPersonnel>(personnel);
                }

                save = true;
            }

            request.SurgeryTicketPersonnels = request.SurgeryTicketPersonnels.DistinctBy(x => x.PersonnelId).ToList();

            if (request.SurgeryTicketPersonnels.Count > 0)
            {
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

                save = true;
            }

            var mailSent = false;

            if (request.SurgeryDate.HasValue && request.SurgeryDate != ticketInventory.SurgeryDate)
            {
                mailSent = true;
                // Send a mail to all staff if there are any
                // This will just save (schedule) it in the database to be send later
            }

            if (ticketInventory.SurgeryDate != request.SurgeryDate)
            {
                ticketInventory.SurgeryDate = request.SurgeryDate;
                iDBRepository.Update<TicketInventory>(ticketInventory);
                save = true;
            }

            if (save)
            {
                await iDBRepository.Complete();
            }

            return new UpdateSurgeryTicketResponse
            {
                MailSent = mailSent,
            };
        }
    }
}
