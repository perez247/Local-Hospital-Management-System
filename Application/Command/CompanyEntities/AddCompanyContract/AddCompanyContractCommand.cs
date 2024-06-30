using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using Application.Utilities.QueryHelpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.CompanyEntities.AddCompanyContract
{
    public class AddCompanyContractCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }
        public int? DurationInDays { get; set; }
        public decimal? Amount { get; set; }
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
            // Get our home company
            var homeCompany = await CompanyHelper.GetHomeCompany(iCompanyRepository);

            var company = await iCompanyRepository.Companies()
                                                  .Include(x => x.CompanyContracts.OrderByDescending(z => z.StartDate).Take(2)).ThenInclude(z => z.AppCost)
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.CompanyId);

            if (company == null)
            {
                throw new CustomMessageException("No company found to add contract");
            }

            var hasContract = company.CompanyContracts.FirstOrDefault();
            var timespan = new TimeSpan(0, 0, 0, 0);

            if (hasContract != null && hasContract.StartDate.AddDays(hasContract.Duration) > DateTime.Now && hasContract.AppCost.PaymentStatus != PaymentStatus.canceled)
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

            AppCost appCost = FinancialHelper.AppCostFactory(company.AppUserId, homeCompany.AppUserId, request.Amount.Value, Description, AppCostType.profit);
            appCost.PaymentStatus = PaymentStatus.owing;
            appCost.CostType = AppCostType.part_ticket;

            newContract.AppCostId = appCost.Id;

            await iDBRepository.AddAsync(appCost);
            await iDBRepository.AddAsync(newContract);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
