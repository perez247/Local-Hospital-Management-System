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

namespace Application.Command.AddPatientContract
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

        public AddPatientContractHandler(IPatientRepository IPatientRepository, IDBRepository IDBRepository, IFinancialRespository IFinancialRespository)
        {
            iPatientRepository = IPatientRepository;
            iDBRepository = IDBRepository;
            iFinancialRespository = IFinancialRespository;
        }
        public async Task<Unit> Handle(AddPatientContractCommand request, CancellationToken cancellationToken)
        {
            // TODO: - Create a setting table to store cost for registration for a patient
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
            var timespan = new TimeSpan(0,0,0,0);

            if (hasContract != null && hasContract.StartDate.AddDays(hasContract.Duration) > DateTime.Now && hasContract.AppCost.PaymentStatus != Models.Enums.PaymentStatus.canceled)
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

            AppCost appCost = CreateAppCost(cost, Description);
            FinancialRecord financialRecord = CreateFinancialCost(cost, Description);

            newContract.AppCostId = appCost.Id;
            appCost.FinancialRecordId = financialRecord.Id;

            await iDBRepository.AddAsync<AppCost>(appCost);
            await iDBRepository.AddAsync<FinancialRecord>(financialRecord);
            await iDBRepository.AddAsync<PatientContract>(newContract);

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

        private static AppCost CreateAppCost(decimal cost, string Description)
        {
            var appCost = new AppCost
            {
                Id = Guid.NewGuid(),
                Amount = cost,
                ApprovedPrice = cost,
                CostType = Models.Enums.AppCostType.profit,
                Description = Description,
            };
            return appCost;
        }
        private static FinancialRecord CreateFinancialCost(decimal cost, string Description)
        {
            var appCost = new FinancialRecord
            {
                Id = Guid.NewGuid(),
                Amount = cost,
                ApprovedAmount = cost,
                CostType = Models.Enums.AppCostType.profit,
                Description = Description,
            };

            return appCost;
        }
    }
}
