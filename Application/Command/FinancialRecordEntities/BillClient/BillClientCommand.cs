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
                                                        .ThenInclude(x => x.TicketInventoryDebtors)
                                                   .Include(x => x.TicketInventories)
                                                        .ThenInclude(a => a.AppInventory)
                                                            .ThenInclude(a => a.AppInventoryItems.Where(b => b.CompanyId == companyPaying.Id))
                                                   .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppTicketId);


            if (appTicket == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            appTicket.MustHvaeBeenSentToDepartment();
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
            var debtors = new List<TicketInventoryDebtor>();
            var description = "Appointment";

            foreach (var x in appTicket.TicketInventories)
            {
                if (x.AppTicketStatus == Models.Enums.AppTicketStatus.ongoing)
                {
                    if (x.AppInventory == null)
                    {
                        throw new CustomMessageException("One of the items to bill was not found in the inventory");
                    }

                    var item = x.AppInventory.AppInventoryItems.FirstOrDefault();

                    if (item == null)
                    {
                        throw new CustomMessageException($"{x.AppInventory.Name} was not found for {companyName}");
                    }

                    var addmissionDays = 1;
                    if (x.AppInventory.AppInventoryType == AppInventoryType.admission)
                    {
                        if (x.AdmissionStartDate == null)
                        {
                            throw new CustomMessageException($"Admission Start date for {x.AppInventory.Name} is required");
                        }

                        if (x.AdmissionEndDate == null)
                        {
                            throw new CustomMessageException($"Admission End date for {x.AppInventory.Name} is required");
                        }

                        addmissionDays = (x.AdmissionEndDate - x.AdmissionStartDate).Value.Days;

                        description = "Admission";
                    }



                    x.TotalPrice = decimal.Parse(x.PrescribedQuantity) * item.PricePerItem * addmissionDays;
                    x.CurrentPrice = item.PricePerItem;

                    //verify sum of all debtor must equal cost provided
                    await CollateDebtorsAmount(debtors, x, payerId.Value, _dBRepository);

                    if (!x.LoggedQuantity.HasValue || (x.LoggedQuantity.HasValue && !x.LoggedQuantity.Value))
                    {
                        FinancialHelper.UpdateQuantity(x, x.AppInventory, int.Parse(x.PrescribedQuantity), request.getCurrentUserRequest().CurrentUser.Id, _dBRepository, nameof(BillClientCommand));
                    
                    }

                    _dBRepository.Update<TicketInventory>(x);
                }
            }

            // Update the financial record
            foreach (var debtor in debtors)
            {
                var appCostForDebtor = FinancialHelper.AppCostFactory(debtor.PayerId, homeCompany.AppUserId, 0, description + ": " + debtor.Description, AppCostType.part_ticket);
                appCostForDebtor.Amount = debtor.Amount;
                appCostForDebtor.ApprovedPrice = debtor.Amount;
                appCostForDebtor.Payments = new List<Payment>();
                appCostForDebtor.PaymentStatus = PaymentStatus.owing;
                appCostForDebtor.AppTicketPartId = appTicket.Id;
                await _dBRepository.AddAsync<AppCost>(appCostForDebtor);
            }

            sumTotal = debtors.Sum(x => x.Amount).Value;
            AppCost newAppCost = FinancialHelper.AppCostFactory(payerId, homeCompany.AppUserId, 0, description, AppCostType.overall_ticket);
            newAppCost.Amount = sumTotal;
            newAppCost.ApprovedPrice = sumTotal;
            newAppCost.Payments = new List<Payment>();
            newAppCost.PaymentStatus = PaymentStatus.owing;
            newAppCost.AppTicketId = appTicket.Id;
            await _dBRepository.AddAsync<AppCost>(newAppCost);

            appTicket.AppCostId = newAppCost.Id;
            _dBRepository.Update<AppTicket>(appTicket);

            // Update payer for appointment
            if (appTicket.Appointment.CompanyId != companyPaying.Id)
            {
                appTicket.Appointment.CompanyId = companyPaying.Id;
                _dBRepository.Update<AppAppointment>(appTicket.Appointment);
            }

            // Save everything

            try
            {
                await _dBRepository.Complete();
            }
            catch (Exception e)
            {

                throw new CustomMessageException(e.Message);
            }

            return Unit.Value;
        }

        private static async Task CollateDebtorsAmount(List<TicketInventoryDebtor> debtors, TicketInventory x, Guid payerId, IDBRepository dBRepository)
        {

            if (x.TicketInventoryDebtors == null || x.TicketInventoryDebtors.Count == 0)
            {
                TicketInventoryDebtor newDebtor = new TicketInventoryDebtor
                {
                    PayerId = payerId,
                    TicketInventoryId = x.Id,
                    Amount = x.ConcludedPrice
                };

                await dBRepository.AddAsync<TicketInventoryDebtor>(newDebtor);
                x.TicketInventoryDebtors = new List<TicketInventoryDebtor>() { newDebtor };
            }

            var debtSum = x.TicketInventoryDebtors.Sum(a => a.Amount);
            if (debtSum == null || debtSum != x.ConcludedPrice)
            {
                throw new CustomMessageException($"Kindly add all the list of debtors for {x.AppInventory.Name} to complete amount to be paid");
            }

            foreach (var debtor in x.TicketInventoryDebtors)
            {
                var foundDebtor = debtors.FirstOrDefault(d => d.PayerId == debtor.PayerId);
                if (foundDebtor != null)
                {
                    foundDebtor.Amount += debtor.Amount;
                    foundDebtor.Description += " - " + x.AppInventory.Name;
                    debtors.Add(foundDebtor);
                }
                else
                {
                    debtor.Description = x.AppInventory.Name;
                    debtors.Add(debtor);
                }
            }
        }
    }
}
