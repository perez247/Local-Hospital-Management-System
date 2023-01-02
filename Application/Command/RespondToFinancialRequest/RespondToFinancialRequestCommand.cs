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

namespace Application.Command.RespondToFinancialRequest
{
    public class RespondToFinancialRequestCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? FinancialRequestId { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Description { get; set; }
        public string? PaymentDetails { get; set; }
        public string? PaymentType { get; set; }
        public string? Proof { get; set; }
    }

    public class RespondToFinancialRequestHandler : IRequestHandler<RespondToFinancialRequestCommand, Unit>
    {
        private readonly IStaffRepository iStaffRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public RespondToFinancialRequestHandler(IStaffRepository IStaffRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iStaffRepository = IStaffRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<Unit> Handle(RespondToFinancialRequestCommand request, CancellationToken cancellationToken)
        {
            var financialRequest = await iStaffRepository.FinancialRequests()
                                                         .FirstOrDefaultAsync(x => x.Id.ToString() == request.FinancialRequestId);

            if (financialRequest == null)
            {
                throw new CustomMessageException("Financial Request not found");
            }

            if (financialRequest.PaymentStatus == PaymentStatus.approved || financialRequest.PaymentStatus == PaymentStatus.canceled)
            {
                throw new CustomMessageException("Request has already been attended to");
            }

            var status = request.PaymentStatus.ParseEnum<PaymentStatus>();

            if (status != PaymentStatus.canceled && status != PaymentStatus.approved)
            {
                throw new CustomMessageException("Status should either be approved or canceled");
            }

            financialRequest.PaymentStatus = status;

            if (financialRequest.PaymentStatus == PaymentStatus.approved )
            {
                var financialRecord = new FinancialRecord
                {
                    Id = financialRequest.Id,
                    Amount = financialRequest.Amount,
                    ApprovedAmount = financialRequest?.Amount,
                    CostType = financialRequest.AppCostType,
                    PaymentStatus = financialRequest.PaymentStatus,
                    Description = request?.Description,
                    Payments = new List<Payment>
                    {
                        new Payment
                        {
                            Amount = financialRequest.Amount.Value,
                            Tax = financialRequest.AppCostType == AppCostType.profit ? financialRequest.Amount.Value * AppTax.Basic : 0,
                            PaymentType = request.PaymentType.ParseEnum<PaymentType>(),
                            PaymentDetails = request.PaymentDetails,
                        }
                    },
                };

                financialRequest.FinancialRecordId = financialRecord.Id;
                await iDBRepository.AddAsync<FinancialRecord>(financialRecord);
            }

            iDBRepository.Update<FinancialRequest>(financialRequest);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
