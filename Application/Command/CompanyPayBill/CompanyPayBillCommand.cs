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

namespace Application.Command.CompanyPayBill
{
    public class CompanyPayBillCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }

        [VerifyGuidCollectionAnnotation]
        public ICollection<string>? AppCostIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? UseId { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? AmountApproved { get; set; }
        public string? PaymentType { get; set; }
        public string? Base64File { get; set; }
        public string? PaymentDetails { get; set; }
        public string? AdditionalDetail { get; set; }
    }

    public class CompanyPayBillHandler : IRequestHandler<CompanyPayBillCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IStaffRepository iStaffRepository;
        private readonly IUserRepository iUserRepository;
        private readonly IDBRepository iDBRepository;

        public CompanyPayBillHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IStaffRepository IStaffRepository, IUserRepository IUserRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iStaffRepository = IStaffRepository;
            iUserRepository = IUserRepository;
        }

        public async Task<Unit> Handle(CompanyPayBillCommand request, CancellationToken cancellationToken)
        {
            var company = await iUserRepository.Companies()
                                         .FirstOrDefaultAsync(x => x.Id.ToString() == request.CompanyId);  
            
            if (company == null)
            {
                throw new CustomMessageException("Company not found");
            }

            var appCosts = new List<AppCost>();

            if (request.UseId.Value)
            {
                request.AppCostIds = request.AppCostIds.Distinct().ToList();
                appCosts = await iStaffRepository.AppCosts()
                                                    .Include(x => x.AppTicket)
                                                        .ThenInclude(x => x.Appointment)
                                                    .Where(x =>
                                                        x.AppTicket.Appointment.CompanyId.ToString() == request.CompanyId &&
                                                        request.AppCostIds.Contains(x.Id.ToString())
                                                    ).ToListAsync();

            } else
            {
                appCosts = await iStaffRepository.AppCosts()
                                                    .Include(x => x.AppTicket)
                                                        .ThenInclude(x => x.Appointment)
                                                    .Where(x =>
                                                        x.AppTicket.Appointment.CompanyId.ToString() == request.CompanyId &&
                                                        x.DateCreated >= request.StartDate && x.DateCreated <= request.EndDate
                                                        )
                                                    .ToListAsync();
            }

            if (appCosts.Count <= 0)
            {
                throw new CustomMessageException("No cost found to edit");
            }
                           
            if (appCosts.Any(x => x.PaymentStatus == Models.Enums.PaymentStatus.approved))
            {
                throw new CustomMessageException("It seems one of the cost has already been approved");
            }

            var financialRecord = new FinancialRecord
            {
                Id = Guid.NewGuid(),
                Amount = request.AmountApproved,
                CostType = Models.Enums.AppCostType.profit,
                PaymentStatus = Models.Enums.PaymentStatus.approved,
                Description = "Paid by company",
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = request.AmountPaid.Value,
                        PaymentType = request.PaymentType.ParseEnum<PaymentType>(),
                        Tax = request.AmountPaid.Value * AppTax.Basic,
                        Proof = request.Base64File,
                        PaymentDetails = request.PaymentDetails,
                        AdditionalDetail = request.AdditionalDetail,
                    }
                }
            };

            var sum = 0m;
            appCosts = appCosts.Select(x =>
            {
                x.PaymentStatus = Models.Enums.PaymentStatus.approved;
                x.Payments = new List<Payment>() { new Payment { Amount = x.ApprovedPrice.Value, PaymentType = Models.Enums.PaymentType.bank_transfer } };
                x.Description = "Paid by company";
                x.FinancialRecordId = financialRecord.Id;
                sum += x.ApprovedPrice.Value;
                return x;
            }).ToList();

            if (sum != request.AmountApproved)
            {
                financialRecord.PaymentStatus = PaymentStatus.owing;
            }

            await iDBRepository.AddAsync<FinancialRecord>(financialRecord);
            iDBRepository.UpdateRange<AppCost>(appCosts);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
