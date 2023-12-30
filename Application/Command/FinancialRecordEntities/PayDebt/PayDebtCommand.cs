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

namespace Application.Command.FinancialRecordEntities.PayDebt
{
    public class PayDebtCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }
        public bool? IsPatient { get; set; }
        public decimal? AmountToPay { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Proof { get; set; }
        public string? PaymentType { get; set; }
    }

    public class PayDebtHandler : IRequestHandler<PayDebtCommand, Unit>
    {
        private IFinancialRespository _financialRespository { get; set; }
        private IUserRepository _userRepository { get; set; }
        private IDBRepository _dBRepository { get; set; }
        private ICompanyRepository _companyRepository { get; set; }

        public PayDebtHandler(IFinancialRespository financialRespository, IUserRepository userRepository, IDBRepository dBRepository, ICompanyRepository companyRepository)
        {
            _financialRespository = financialRespository;
            _userRepository = userRepository;
            _dBRepository = dBRepository;
            _companyRepository = companyRepository;
        }
        public async Task<Unit> Handle(PayDebtCommand request, CancellationToken cancellationToken)
        {
            var homeCompany = await _companyRepository.Companies()
                                                .FirstOrDefaultAsync(x => x.HomeCompany);

            if (homeCompany == null)
            {
                throw new CustomMessageException("Home Company is required");
            }

            var tax = await _financialRespository.GetTax();

            var query = _financialRespository.GetAppCosts()
                                                  .Include(x => x.FinancialRecordPayerPayees)
                                                  .Include(x => x.AppTicket)
                                                  .Where(x => x.FinancialRecordPayerPayees.FirstOrDefault(y => y.AppUserId.ToString() == request.UserId && y.Payer) != null)
                                                  .Where(x => x.PaymentStatus == Models.Enums.PaymentStatus.owing);
                                                  
            if (request.StartDate.HasValue)
            {
                query = query.Where(x => x.DateCreated >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(x => x.DateCreated <= request.EndDate.Value);
            }

            var costs = await query.ToListAsync();

            if (costs.Count <= 0)
            {
                throw new CustomMessageException("No Debt found");
            }

            var sum = 0m;

            var financialId = Guid.NewGuid();

            foreach (var cost in costs)
            {
                sum += cost.ApprovedPrice.Value;
                cost.PaymentStatus = Models.Enums.PaymentStatus.approved;
                cost.FinancialRecordId = financialId;

                if (cost.Payments.Count > 0)
                {
                    var costSum = 0m;
                    cost.Payments.Select(x =>
                    {
                        costSum += x.Amount;
                        return costSum;
                    });

                    cost.Payments.Add(new Payment
                    {
                        Amount = cost.ApprovedPrice.Value - costSum,
                        PaymentType = request.PaymentType.ParseEnum<PaymentType>(),
                        DatePaid = DateTime.Now,
                        Proof = request.Proof,
                        Tax = (cost.ApprovedPrice.Value - costSum) * tax
                    });

                } else
                {
                    cost.Payments = new List<Payment>
                    {
                        new Payment {
                            Amount = cost.ApprovedPrice.Value, 
                            PaymentType = request.PaymentType.ParseEnum<PaymentType>(),
                            DatePaid = DateTime.Now,
                            Proof = request.Proof,
                            Tax = cost.ApprovedPrice.Value * tax
                        }
                    };
                }

                _dBRepository.Update<AppCost>(cost);
            }

            var newFinancialRecord = new FinancialRecord
            {
                Id = financialId,
                Amount = sum,
                ApprovedAmount = request.AmountToPay,
                PaymentStatus = PaymentStatus.approved,
                CostType = request.UserId == homeCompany.AppUserId.ToString() ? Models.Enums.AppCostType.expense : Models.Enums.AppCostType.profit,
                ActorId = request.getCurrentUserRequest().CurrentUser.Id,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = request.AmountToPay.Value,
                        Tax = request.AmountToPay.Value * tax,
                        Proof = request.Proof,
                        PaymentType = request.PaymentType.ParseEnum<PaymentType>(),
                        DatePaid = DateTime.Now,
                    }
                },
                FinancialRecordPayerPayees = new List<FinancialRecordPayerPayee>
                {
                    new FinancialRecordPayerPayee
                    {
                        AppUserId = Guid.Parse(request.UserId),
                        Payer = true,
                    },
                    new FinancialRecordPayerPayee
                    {
                        AppUserId = homeCompany.AppUserId
                    },
                }
            };

            await _dBRepository.AddAsync<FinancialRecord>(newFinancialRecord);

            await _dBRepository.Complete();

            return Unit.Value;
        }
    }
}
