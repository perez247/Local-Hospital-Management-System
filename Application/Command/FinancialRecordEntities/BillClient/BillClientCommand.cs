using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using Application.Utilities.QueryHelpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.FinancialRecordEntities.BillClient
{
    public class BillClientCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }

        [VerifyGuidAnnotation]
        public string? AppTicketId { get; set; }
    }

    public class BillClientHandler : IRequestHandler<BillClientCommand, Unit>
    {
        private ICompanyRepository? _companyRepository { get; set; }
        private ITicketRepository? _ticketRepository { get; set; }
        private IFinancialRespository? _financialRespository { get; set; }
        private IDBRepository? _dBRepository { get; set; }

        IDictionary<string, string> d = new Dictionary<string, string>();

        public int Count => d.Count;

        public BillClientHandler(IDBRepository? dBRepository, IFinancialRespository? financialRespository, ITicketRepository? ticketRepository, ICompanyRepository? companyRepository)
        {
            _dBRepository = dBRepository;
            _financialRespository = financialRespository;
            _ticketRepository = ticketRepository;
            _companyRepository = companyRepository;
        }
        public async Task<Unit> Handle(BillClientCommand request, CancellationToken cancellationToken)
        {
            decimal vat = await _financialRespository.GetTax();

            // Get home company
            var homeCompany = await CompanyHelper.GetHomeCompany(_companyRepository);

            // Get home company
            var individualCompany = await CompanyHelper.GetIndividualCompany(_companyRepository);


            // get the company paying
            var companyPaying = await _companyRepository.Companies()
                                                .Include(x => x.AppUser)
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.CompanyId);

            Guid? payerId;
            var companyName = "";
            if (companyPaying == null)
            {
                throw new CustomMessageException("Payee not found");
            }

            // Get ticket and inventories
            var appTicket = await _ticketRepository.AppTickets()
                                                   .Include(x => x.Appointment)
                                                        .ThenInclude(x => x.Patient)
                                                            .ThenInclude(x => x.Company)
                                                                .ThenInclude(x => x.CompanyContracts.OrderByDescending(y => y.DateCreated).Take(1))
                                                   .Include(x => x.TicketInventories)
                                                        .ThenInclude(a => a.AppInventory)
                                                            .ThenInclude(a => a.AppInventoryItems.Where(b => b.CompanyId == companyPaying.Id))
                                                   .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppTicketId);


            if (appTicket == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            appTicket.MustNotHaveBeenSentToDepartment();
            appTicket.MustHaveBeenSentToFinance();
            
            if (appTicket.AppTicketStatus != AppTicketStatus.ongoing)
            {
                throw new CustomMessageException("Ticket must be ongoing");
            }

            // Check the companies to make payment
            if (companyPaying.Id == individualCompany.Id)
            {
                payerId = appTicket.Appointment.Patient.AppUserId;
                companyName = individualCompany.AppUser.FirstName;
            } else if (appTicket.Appointment.Patient.CompanyId == companyPaying.Id)
            {
                payerId = companyPaying.AppUserId;
                companyName = companyPaying.AppUser.FirstName;
            } else
            {
                throw new CustomMessageException("Payee was unable to be determined");
            }

            // Make sure at least one ticketinventory is ongoing
            var ongoings = appTicket.TicketInventories.Count(x => x.AppTicketStatus == AppTicketStatus.ongoing);

            if (ongoings == 0)
            {
                throw new CustomMessageException("At least one item should be ongoing");
            }

            // Get the sumtotal
            decimal sumTotal = 0.0m;

            foreach (var x in appTicket.TicketInventories)
            {
                if (x.AppTicketStatus == Models.Enums.AppTicketStatus.ongoing)
                {
                    if (x.AppInventory == null)
                    {
                        throw new CustomMessageException("One the items to bill was not found in the inventory");
                    }

                    var item = x.AppInventory.AppInventoryItems.FirstOrDefault();

                    if (item == null)
                    {
                        throw new CustomMessageException($"{x.AppInventory.Name} was not found for {companyName}");
                    }

                    x.TotalPrice = decimal.Parse(x.PrescribedQuantity) * item.PricePerItem;
                    x.CurrentPrice = item.PricePerItem;
                    sumTotal += x.TotalPrice.Value;

                    if (!x.LoggedQuantity.HasValue || (x.LoggedQuantity.HasValue && !x.LoggedQuantity.Value))
                    {
                        var oldQuantity = x.AppInventory.Quantity;

                        FinancialHelper.UpdateQuantity(x, x.AppInventory, int.Parse(x.PrescribedQuantity));

                        if (oldQuantity != x.AppInventory.Quantity)
                        {
                            _dBRepository.Update<AppInventory>(x.AppInventory);
                        }
                    }

                    _dBRepository.Update<TicketInventory>(x);
                }
            }

            // Update the financial record

            sumTotal = Math.Round(sumTotal, 2);
            AppCost newAppCost = FinancialHelper.AppCostFactory(payerId, homeCompany.AppUserId, 0, "", AppCostType.profit);
            newAppCost.Amount = sumTotal;
            newAppCost.ApprovedPrice = sumTotal;
            newAppCost.Payments = new List<Payment>();
            newAppCost.PaymentStatus = PaymentStatus.owing;
            await _dBRepository.AddAsync<AppCost>(newAppCost);

            appTicket.AppCostId = newAppCost.Id;
            _dBRepository.Update<AppTicket>(appTicket);

            // Save everything

            await _dBRepository.Complete();

            return Unit.Value;
        }
    }
}
