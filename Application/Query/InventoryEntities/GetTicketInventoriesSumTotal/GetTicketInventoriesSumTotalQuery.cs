using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetTicketInventoriesSumTotal
{
    public class GetTicketInventoriesSumTotalQuery : TokenCredentials, IRequest<decimal>
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }

        [VerifyGuidAnnotation]
        public string? AppTicketId { get; set; }
    }

    public class GetTicketInventoriesSumTotalHandler : IRequestHandler<GetTicketInventoriesSumTotalQuery, decimal>
    {
        private ICompanyRepository? _companyRepository { get; set; }
        private ITicketRepository? _ticketRepository { get; set; }

        public GetTicketInventoriesSumTotalHandler(ITicketRepository? ticketRepository, ICompanyRepository? companyRepository = null)
        {
            _ticketRepository = ticketRepository;
            _companyRepository = companyRepository;
        }

        public async Task<decimal> Handle(GetTicketInventoriesSumTotalQuery request, CancellationToken cancellationToken)
        {

            // get the company paying
            var companyPaying = await _companyRepository.Companies()
                                                  .Include(x => x.AppUser)
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.CompanyId);


            if (companyPaying == null)
            {
                throw new CustomMessageException("Company to pay not found");
            }

            // Get ticket and inventories
            var appTicket = await _ticketRepository.AppTickets()
                                                   .Include(x => x.TicketInventories)
                                                        .ThenInclude(a => a.AppInventory)
                                                            .ThenInclude(a => a.AppInventoryItems.Where(b => b.CompanyId == companyPaying.Id))
                                                   .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppTicketId);


            if (appTicket == null)
            {
                throw new CustomMessageException("Ticket was not found");
            }


            // Get the sumtotal
            decimal sumTotal = 0.0m;

            foreach (var ticketInventory in appTicket.TicketInventories)
            {
                if (ticketInventory.AppTicketStatus == Models.Enums.AppTicketStatus.ongoing)
                {
                    var inventory = ticketInventory.AppInventory;
                    var item = inventory.AppInventoryItems.FirstOrDefault();

                    if (item == null)
                    {
                        throw new CustomMessageException($"{companyPaying.AppUser.FirstName} price for {inventory.Name} not found");
                    }


                    var addmissionDays = 1;
                    if (ticketInventory.AppInventory.AppInventoryType == AppInventoryType.admission)
                    {
                        ticketInventory.AdmissionStartDate = ticketInventory.AdmissionStartDate.HasValue ? ticketInventory.AdmissionStartDate : DateTime.Today;
                        ticketInventory.AdmissionEndDate = ticketInventory.AdmissionEndDate.HasValue ? ticketInventory.AdmissionEndDate : DateTime.Today;
                        if (ticketInventory.AdmissionEndDate == null)
                        {
                            throw new CustomMessageException($"Admission End date for {ticketInventory.AppInventory.Name} is required");
                        }

                        addmissionDays = Math.Abs((ticketInventory.AdmissionEndDate - ticketInventory.AdmissionStartDate).Value.Days);
                    }

                    if (ticketInventory.ConcludedPrice.HasValue)
                    {
                        sumTotal += ticketInventory.ConcludedPrice.Value;
                    } else
                    {
                        sumTotal += item.PricePerItem * Int32.Parse(ticketInventory.PrescribedQuantity) * addmissionDays;
                    }

                }
            }

            return sumTotal;
        }
    }
}
