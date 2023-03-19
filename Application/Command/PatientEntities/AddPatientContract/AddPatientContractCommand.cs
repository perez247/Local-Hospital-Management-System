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

namespace Application.Command.PatientEntities.AddPatientContract
{
    public class AddPatientContractCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }
        public int? DurationInDays { get; set; }
    }

    public class AddPatientContractHandler : IRequestHandler<AddPatientContractCommand, Unit>
    {
        private readonly IPatientRepository iPatientRepository;
        private readonly IDBRepository iDBRepository;
        private readonly IFinancialRespository iFinancialRespository;
        private ICompanyRepository? _companyRepository { get; set; }

        public AddPatientContractHandler(IPatientRepository IPatientRepository, IDBRepository IDBRepository, IFinancialRespository IFinancialRespository, ICompanyRepository? companyRepository)
        {
            iPatientRepository = IPatientRepository;
            iDBRepository = IDBRepository;
            iFinancialRespository = IFinancialRespository;
            _companyRepository = companyRepository;
        }
        public async Task<Unit> Handle(AddPatientContractCommand request, CancellationToken cancellationToken)
        {
            // Get our home company
            var homeCompany = await CompanyHelper.GetHomeCompany(_companyRepository);

            var cost = await iFinancialRespository.GetPatientContractCost();
            var patient = await iPatientRepository.Patients()
                                                .Include(x => x.Company)
                                                .Include(y => y.PatientContracts.OrderByDescending(z => z.StartDate).Take(2)).ThenInclude(z => z.AppCost)
                                                .FirstOrDefaultAsync(x => x.Id.ToString() == request.PatientId);

            if (patient == null)
            {
                throw new CustomMessageException("No patient found to add contract");
            }

            if (patient.Company != null && !patient.Company.ForIndividual)
            {
                throw new CustomMessageException("Patient is part of a company");
            }

            var hasContract = patient.PatientContracts.FirstOrDefault();
            var timespan = new TimeSpan(0, 0, 0, 0);

            if (hasContract != null && hasContract.StartDate.AddDays(hasContract.Duration) > DateTime.Now && hasContract.AppCost.PaymentStatus != PaymentStatus.canceled)
            {
                timespan = hasContract.StartDate.AddDays(hasContract.Duration) - DateTime.Now;
                var timspnInDays = timespan.Days;

                if (timspnInDays > 14)
                {
                    throw new CustomMessageException("Patient already has a contract");
                }
            }

            var Description = "First Registration";

            if (hasContract != null)
            {
                Description = "Renew Contract";
            }

            var durationInDays = request.DurationInDays.Value + timespan.Days + 1;

            var newContract = new PatientContract
            {
                PatientId = patient.Id,
                StartDate = DateTime.Today.AddDays(1).ToUniversalTime(),
                Duration = durationInDays,
            };

            AppCost appCost = FinancialHelper.AppCostFactory(patient.AppUserId, homeCompany.AppUserId, cost, Description, AppCostType.profit);
            appCost.PaymentStatus = PaymentStatus.owing;

            newContract.AppCostId = appCost.Id;

            await iDBRepository.AddAsync(appCost);
            await iDBRepository.AddAsync(newContract);

            try
            {
                await iDBRepository.Complete();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return Unit.Value;
        }
    }
}
