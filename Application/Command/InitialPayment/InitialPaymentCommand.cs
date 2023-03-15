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

namespace Application.Command.InitialPayment
{
    public class InitialPaymentCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }

        [VerifyGuidAnnotation]
        public string? AppTicketId { get; set; }
        public decimal? VatTotal { get; set; }
        public decimal? Total { get; set; }
        public decimal? SumTotal { get; set; }
        public ICollection<InitialPaymentTicketInventory>? TicketInventories { get; set; }
        public ICollection<InitialPaymentPayment>? Payments { get; set; }
    }

    public class InitialPaymentHandler : IRequestHandler<InitialPaymentCommand, Unit>
    {
        private ICompanyRepository? _companyRepository { get; set; }
        private ITicketRepository? _ticketRepository { get; set; }
        private IFinancialRespository? _financialRespository { get; set; }
        private IDBRepository? _dBRepository { get; set; }

        public InitialPaymentHandler(IDBRepository? dBRepository, IFinancialRespository? financialRespository, ITicketRepository? ticketRepository, ICompanyRepository? companyRepository)
        {
            _dBRepository = dBRepository;
            _financialRespository = financialRespository;
            _ticketRepository = ticketRepository;
            _companyRepository = companyRepository;
        }

        public async Task<Unit> Handle(InitialPaymentCommand request, CancellationToken cancellationToken)
        {
            request.VatTotal = Math.Round(request.VatTotal.Value, 2);
            request.Total = Math.Round(request.Total.Value, 2);
            request.SumTotal = Math.Round(request.SumTotal.Value, 2);
            decimal vat = await _financialRespository.GetTax();
            // get the company paying
            var companyPaying = await _companyRepository.Companies()
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.CompanyId);
            Guid payeeId;
            if (companyPaying == null)
            {
                throw new CustomMessageException("Payee not found");
            }

            // Get ticket and inventories
            var appTicket = await _ticketRepository.AppTickets()
                                                   .Include(x => x.Appointment)
                                                   .Include(x => x.TicketInventories)
                                                        .ThenInclude(a => a.AppInventory)
                                                   .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppTicketId);

            
            // make sure ticket has been send to dept and finance
            appTicket.MustHvaeBeenSentToDepartment();
            appTicket.MustHaveBeenSentToFinance();

            // make sure ticket is onging
            if (appTicket.AppTicketStatus != Models.Enums.AppTicketStatus.ongoing)
            {
                throw new CustomMessageException("Ticket must be ongoing");
            }

            // make sure at list one is ongoing
            var atLeastOneOnging = appTicket.TicketInventories.Where(x => x.AppTicketStatus == Models.Enums.AppTicketStatus.ongoing).ToList();
            if (atLeastOneOnging.Count <= 0)
            {
                throw new CustomMessageException("At least one ticket inventory should be ongoing");
            }

            // check if it is an individual or a company
            if (companyPaying.ForIndividual)
            {
                // if individual
                // check that the indiviaul is not owing up to 10 
                var ticketsOwing = await _ticketRepository.AppTickets()
                                                          .Include(x => x.AppCost)
                                                          .Where(x => x.AppCost != null && x.AppCost.PaymentStatus == Models.Enums.PaymentStatus.owing)
                                                          .ToListAsync();

                if (ticketsOwing.Count > 11)
                {
                    throw new CustomMessageException("An Individual cannot owe more than 10 tickets at a time, kindly pay for some tickets");
                }

                appTicket.PatientId = appTicket.Appointment.PatientId.Value;
            } else
            {
                appTicket.CompanyId = companyPaying.Id.Value;
            }

            // Get the sum of all ongoing ticket inventory total price from the request
            var sumTotal = request.TicketInventories.Where(x => x.AppTicketStatus.ParseEnum<AppTicketStatus>() == AppTicketStatus.ongoing).Sum(a => a.TotalPrice) + request.VatTotal;
            sumTotal = Math.Round(sumTotal.Value, 2);
            // total is not equal to sumTotal given from request then throw error, it should be equal
            if (sumTotal != request.SumTotal)
            {
                throw new CustomMessageException("Sum total given is not equal to calculated from ticket inventory");
            }


            // update all ticketInventory 
            // total price,
            // current price
            // status
            foreach (var ticketInventory in appTicket.TicketInventories)
            {
                var requestTicketInventory = request.TicketInventories.FirstOrDefault(x => x.TicketInventoryId == ticketInventory.Id.ToString());
                
                if (requestTicketInventory == null)
                {
                    throw new CustomMessageException($"{ticketInventory.AppInventory.Name} is missing from the update");
                }

                if (ticketInventory.AppInventory == null)
                {
                    throw new CustomMessageException($"{ticketInventory.AppInventory.Name} is missing from the inventory");
                }

                ticketInventory.TotalPrice = Math.Round(requestTicketInventory.TotalPrice.Value, 2);
                ticketInventory.CurrentPrice = Math.Round(requestTicketInventory.CurrentPrice.Value, 2);
                ticketInventory.AppTicketStatus = requestTicketInventory.AppTicketStatus.ParseEnum<AppTicketStatus>();
                _dBRepository.Update<TicketInventory>(ticketInventory);

                ticketInventory.AppInventory.Quantity = ticketInventory.AppInventory.Quantity - requestTicketInventory.PrescribedQuantity.Value;
                _dBRepository.Update<AppInventory>(ticketInventory.AppInventory);
            }

            // Create an app cost for the ticket
            // sum amount from current price = Amount + tax
            // set approved price = sumTotal
            var newAppCost = new AppCost
            {
                Id = Guid.NewGuid(),
                Amount = request.TicketInventories.Where(x => x.AppTicketStatus.ParseEnum<AppTicketStatus>() == AppTicketStatus.ongoing).Sum(a => a.CurrentPrice) + request.VatTotal,
                ApprovedPrice = request.SumTotal,
                CostType = AppCostType.profit,
            };

            // Add payments
            newAppCost.Payments = request.Payments != null ?
                request.Payments.Select(x => new Payment { Amount = Math.Round(x.Amount.Value, 2), Proof = x.Base64String, PaymentType = x.PaymentType.ParseEnum<PaymentType>(), Tax = Math.Round(x.Amount.Value, 2) * vat, DatePaid = DateTime.Now.ToUniversalTime() }).ToList() : 
                new List<Payment>();

            var paymentSum = newAppCost.Payments.Count > 0 ? newAppCost.Payments.Sum(x => x.Amount) : 0;

            // if sum of payment status > approved price then
            if (paymentSum > newAppCost.ApprovedPrice)
            {
                // throw an error
                throw new CustomMessageException("Payments made is greater than approved price");
            }
            // If sum of payments == approved price then
            if (paymentSum == newAppCost.ApprovedPrice)
            {
                // update payment status = paid
                newAppCost.PaymentStatus = PaymentStatus.approved;
            } else
            {
                // Client is still owing
                newAppCost.PaymentStatus = PaymentStatus.owing;
            }

            // Create new Financial record
            var newfinancialRecord = new FinancialRecord
            {
                Id = Guid.NewGuid(),
                Amount = newAppCost.Amount,
                ApprovedAmount = newAppCost.ApprovedPrice,
                CostType = newAppCost.CostType,
                PaymentStatus = newAppCost.PaymentStatus,
                Payments = newAppCost.Payments,
            };

            newAppCost.FinancialRecordId = newfinancialRecord.Id;

            await _dBRepository.AddAsync<AppCost>(newAppCost);
            await _dBRepository.AddAsync<FinancialRecord>(newfinancialRecord);

            appTicket.AppCostId = newAppCost.Id;

            _dBRepository.Update<AppTicket>(appTicket);

            await _dBRepository.Complete();

            return Unit.Value;
        }
    }
}
