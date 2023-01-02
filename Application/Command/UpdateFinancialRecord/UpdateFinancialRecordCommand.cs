using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Constants;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateFinancialRecord
{
    public class UpdateFinancialRecordCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? FinancialRecordId { get; set; }
        public decimal? AmountPaid { get; set; }
        public string? PaymentType { get; set; }
        public string? Base64File { get; set; }
        public string? PaymentDetails { get; set; }
        public string? AdditionalDetail { get; set; }
    }

    public class UpdateFinancialRecordHandler : IRequestHandler<UpdateFinancialRecordCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IStaffRepository iStaffRepository;
        private readonly IUserRepository iUserRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateFinancialRecordHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IStaffRepository IStaffRepository, IUserRepository IUserRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iStaffRepository = IStaffRepository;
            iUserRepository = IUserRepository;
        }

        public async Task<Unit> Handle(UpdateFinancialRecordCommand request, CancellationToken cancellationToken)
        {
            var financialRecord = await iStaffRepository.FinancialRecords()
                                                        .FirstOrDefaultAsync(x => x.Id.ToString() == request.FinancialRecordId);

            if (financialRecord == null)
            {
                throw new CustomMessageException("Financial Record not found");
            }

            var sum = financialRecord.Payments.Sum(x => x.Amount);
            sum += request.AmountPaid.Value;

            if (sum > financialRecord.ApprovedAmount)
            {
                throw new CustomMessageException("Total amount should not be greater than amount given");
            }

            var payment = new Payment
            {
                Amount = request.AmountPaid.Value,
                PaymentType = request.PaymentType.ParseEnum<PaymentType>(),
                Tax = request.AmountPaid.Value * AppTax.Basic,
                Proof = request.Base64File,
                PaymentDetails = request.PaymentDetails,
                AdditionalDetail = request.AdditionalDetail,
            };

            if (sum == financialRecord.ApprovedAmount.Value)
            {
                financialRecord.PaymentStatus = PaymentStatus.approved;
            } else
            {
                financialRecord.PaymentStatus = PaymentStatus.owing;
            }

            if (financialRecord.Payments == null)
            {
                financialRecord.Payments = new List<Payment>
                {
                    payment
                };
            } else
            {
                financialRecord.Payments.Add(payment);
            }

            iDBRepository.Update<FinancialRecord>(financialRecord);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
