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

namespace Application.Command.UpdateAppCost
{
    public class UpdateAppCostCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? AppCostId { get; set; }
        public decimal? AmountPaid { get; set; }
        public string? PaymentType { get; set; }
        public string? Base64File { get; set; }
        public string? PaymentDetails { get; set; }
        public string? AdditionalDetail { get; set; }
    }

    public class UpdateAppCostHandler : IRequestHandler<UpdateAppCostCommand, Unit>
    {
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateAppCostHandler(IStaffRepository IStaffRepository, IDBRepository IDBRepository)
        {
            iStaffRepository = IStaffRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(UpdateAppCostCommand request, CancellationToken cancellationToken)
        {
            var appCost = await iStaffRepository.AppCosts()
                                                .Include(x => x.AppTicket)
                                                .Include(x => x.FinancialRecord)
                                                .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppCostId);


            if (appCost == null)
            {
                throw new CustomMessageException("Cost not found");
            }

            if (appCost.AppTicket.AppTicketStatus == AppTicketStatus.concluded || appCost.AppTicket.AppTicketStatus == AppTicketStatus.canceled)
            {
                throw new CustomMessageException("This ticket has been concluded or canceled");
            }

            if (appCost.PaymentStatus == PaymentStatus.approved || appCost.PaymentStatus == PaymentStatus.canceled)
            {
                throw new CustomMessageException("This ticket has been approved/canceled ");
            }

            var payments = appCost.Payments;

            var sum = payments.Sum(x => x.Amount) + request.AmountPaid;

            if (sum > appCost.ApprovedPrice)
            {
                throw new CustomMessageException("Amount paid is greater than the approved amount");
            }

            var newPayment = new Payment
            {
                Amount = request.AmountPaid.Value,
                PaymentType = request.PaymentType.ParseEnum<PaymentType>(),
                Tax = request.AmountPaid.Value * AppTax.Basic,
                Proof = request.Base64File,
                PaymentDetails = request.PaymentDetails,
                AdditionalDetail = request.AdditionalDetail,
            };

            appCost.Payments.Add(newPayment);

            var financialRecord = appCost.FinancialRecord;

            if (financialRecord == null)
            {
                financialRecord = new FinancialRecord
                {
                    Id = Guid.NewGuid(),
                    Amount = appCost.ApprovedPrice,
                    ApprovedAmount = appCost.ApprovedPrice,
                    CostType = appCost.CostType,
                    PaymentStatus = appCost.PaymentStatus,
                };
                UpdatePaymentStatus(appCost, sum, financialRecord);
                financialRecord.Payments.Add(newPayment);
                await iDBRepository.AddAsync<FinancialRecord>(financialRecord);
            }
            else
            {
                financialRecord.Amount = appCost.ApprovedPrice;
                financialRecord.ApprovedAmount = appCost.ApprovedPrice;
                UpdatePaymentStatus(appCost, sum, financialRecord);
                financialRecord.Payments.Add(newPayment);
                iDBRepository.Update<FinancialRecord>(financialRecord);
            }

            if (!appCost.FinancialApproverId.HasValue)
            {
                appCost.FinancialApproverId = request.getCurrentUserRequest().CurrentUser.Staff.Id;
            }

            appCost.FinancialRecordId = financialRecord.Id;


            iDBRepository.Update<AppCost>(appCost);
            await iDBRepository.Complete();

            return Unit.Value;
        }

        private static void UpdatePaymentStatus(AppCost? appCost, decimal? sum, FinancialRecord? financialRecord)
        {
            if (sum == appCost.ApprovedPrice)
            {
                appCost.PaymentStatus = PaymentStatus.approved;
                financialRecord.PaymentStatus = PaymentStatus.approved;
            }
            else
            {
                appCost.PaymentStatus = PaymentStatus.owing;
                financialRecord.PaymentStatus = PaymentStatus.owing;
            }
        }
    }
}
