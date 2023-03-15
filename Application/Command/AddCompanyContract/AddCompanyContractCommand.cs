using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddCompanyContract
{
    public class AddCompanyContractCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }
        public int? DurationInDays { get; set; }
    }

    public class AddCompanyContactHandler : IRequestHandler<AddCompanyContractCommand, Unit>
    {
        private readonly IDBRepository iDBRepository;
        private readonly ICompanyRepository iCompanyRepository;
        private readonly IFinancialRespository _iFinancialRespository;

        public AddCompanyContactHandler(IDBRepository IDBRepository, ICompanyRepository ICompanyRepository, IFinancialRespository iFinancialRespository)
        {
            iDBRepository = IDBRepository;
            iCompanyRepository = ICompanyRepository;
            _iFinancialRespository = iFinancialRespository;
        }

        public async Task<Unit> Handle(AddCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var cost = await _iFinancialRespository.CompanyContractCost();
            var company = await iCompanyRepository.Companies()
                                                  .Include(x => x.CompanyContracts.OrderByDescending(z => z.StartDate).Take(2)).ThenInclude(z => z.AppCost)
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.CompanyId);

            if (company == null)
            {
                throw new CustomMessageException("No company found to add contract");
            }

            var hasContract = company.CompanyContracts.FirstOrDefault();
            var timespan = new TimeSpan(0, 0, 0, 0);

            if (hasContract != null && hasContract.StartDate.AddDays(hasContract.Duration) > DateTime.Now && hasContract.AppCost.PaymentStatus != Models.Enums.PaymentStatus.canceled)
            {
                timespan = hasContract.StartDate.AddDays(hasContract.Duration) - DateTime.Now;
                var timspnInDays = timespan.Days;

                if (timspnInDays > 14)
                {
                    throw new CustomMessageException("Company already has a contract");
                }
            }

            var Description = "First Registration";

            if (hasContract != null)
            {
                Description = "Renew Contract";
            }

            var durationInDays = request.DurationInDays.Value + timespan.Days + 1;

            var newContract = new CompanyContract
            {
                CompanyId = company.Id,
                StartDate = DateTime.Today.AddDays(1).ToUniversalTime(),
                Duration = durationInDays,
            };

            var appCost = new AppCost
            {
                Id = Guid.NewGuid(),
                Amount = cost,
                CostType = Models.Enums.AppCostType.profit,
                Description = Description
            };

            var financialRecord = new FinancialRecord
            {
                Id = Guid.NewGuid(),
                Amount = cost,
                ApprovedAmount = cost,
                CostType = Models.Enums.AppCostType.profit,
                Description = Description,
            };


            newContract.AppCostId = appCost.Id;
            appCost.FinancialRecordId = financialRecord.Id;

            await iDBRepository.AddAsync<AppCost>(appCost);
            await iDBRepository.AddAsync<CompanyContract>(newContract);
            await iDBRepository.AddAsync<FinancialRecord>(financialRecord);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
