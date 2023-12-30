using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utilities.QueryHelpers;

namespace Application.Command.FinancialRecordEntities.PatientUpdatePayment
{
    public class PatientUpdatePaymentCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? AppTicketId { get; set; }
        public ICollection<PatientUpdatePaymentPayments>? Payments { get; set; }
    }

    public class PatientUpdatePaymentHandler : IRequestHandler<PatientUpdatePaymentCommand, Unit>
    {
        private ITicketRepository? _ticketRepository { get; set; }
        private IDBRepository? _dBRepository { get; set; }
        private IFinancialRespository? _financialRespository { get; set; }

        public PatientUpdatePaymentHandler(IDBRepository? dBRepository, ITicketRepository? ticketRepository, IFinancialRespository? financialRespository)
        {
            _dBRepository = dBRepository;
            _ticketRepository = ticketRepository;
            _financialRespository = financialRespository;
        }
        public async Task<Unit> Handle(PatientUpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            decimal vat = await _financialRespository.GetTax();
            var ticket = await _ticketRepository.AppTickets()
                                                .Include(x => x.AppCost)
                                                    .ThenInclude(x => x.FinancialRecordPayerPayees)
                                                .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppTicketId);

            if (ticket == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            if (ticket.AppCost == null)
            {
                throw new CustomMessageException("No payments have been made for this ticket");
            }

            if (ticket.AppCost.PaymentStatus != PaymentStatus.owing)
            {
                throw new CustomMessageException("Payment status must be in owing to make anymore payments");
            }

            var paymentSum = request.Payments.Sum(x => Math.Round(x.Amount.Value, 2));
            var currentlyPaid = ticket.AppCost.Payments.Sum(x => x.Amount);


            if (paymentSum + currentlyPaid > ticket.AppCost.ApprovedPrice)
            {
                throw new CustomMessageException("Payments to be made is greater than what is billed");
            }

            var newPayments = request.Payments.Select(x => new Payment { Amount = Math.Round(x.Amount.Value, 2), Proof = x.Base64String, PaymentType = x.PaymentType.ParseEnum<PaymentType>(), Tax = Math.Round(x.Amount.Value, 2) * vat, DatePaid = DateTime.Now.ToUniversalTime() }).ToList();

            var newUnion = ticket.AppCost.Payments.Union(newPayments);
            ticket.AppCost.Payments = newUnion.ToList();

            if (paymentSum + currentlyPaid == ticket.AppCost.ApprovedPrice)
            {
                ticket.AppCost.PaymentStatus = PaymentStatus.approved;

                var financialRecord = FinancialHelper.AppCostToFinancialRecord(ticket.AppCost);
                financialRecord.Payments = ticket.AppCost.Payments;

                foreach (var item in financialRecord.FinancialRecordPayerPayees)
                {
                    await _dBRepository.AddAsync<FinancialRecordPayerPayee>(item);
                }

                ticket.AppCost.FinancialRecordId = financialRecord.Id;
                await _dBRepository.AddAsync<FinancialRecord>(financialRecord);
            }

            _dBRepository.Update(ticket.AppCost);

            await _dBRepository.Complete();

            return Unit.Value;
        }
    }
}
