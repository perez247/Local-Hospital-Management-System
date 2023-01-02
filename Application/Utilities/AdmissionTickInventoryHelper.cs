using Application.Command.AddAdmissionTicketInventory;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class AdmissionTickInventoryHelper
    {
        public static async Task AddAdmissionTicket(
            AddAdmissionTicketInventoryCommand request,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
             IDBRepository iDBRepository
            )
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                                      .Include(x => x.TicketInventories.Take(1))
                                                      .FirstOrDefaultAsync(x => x.Id.ToString() == request.TicketId);

            ticketFromDb.MustNotHaveBeenSentToDepartment();

            var lastAdmission = ticketFromDb.TicketInventories.FirstOrDefault();

            if (lastAdmission != null)
            {
                throw new CustomMessageException("Only one ticket inventory can be created for admission");
            }

            var inventory = await iInventoryRepository.AppInventories()
                                                      .FirstOrDefaultAsync(x => x.Id.ToString() == request.InventoryId);

            if (inventory == null)
            {
                throw new CustomMessageException("Admission room not found");
            }

            var admissionTicket = new TicketInventory
            {
                AppInventoryId = inventory.Id,
                AppTicketId = ticketFromDb.Id,
                AdmissionStartDate = request.StartDate,
                CurrentPrice = request.CurrentPrice,
            };

            await iDBRepository.AddAsync<TicketInventory>(admissionTicket);
        }
    }
}
