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
        public int? DurationInMonths { get; set; }
    }

    public class AddPatientContractHandler : IRequestHandler<AddPatientContractCommand, Unit>
    {
        private readonly IPatientRepository iPatientRepository;
        private readonly IDBRepository iDBRepository;

        public AddPatientContractHandler(IPatientRepository IPatientRepository, IDBRepository IDBRepository)
        {
            iPatientRepository = IPatientRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(AddPatientContractCommand request, CancellationToken cancellationToken)
        {
            var cost = 1500;
            var patient = await iPatientRepository.Patients()
                                                .Include(x => x.Company)
                                                .Include(y => y.PatientContracts.OrderByDescending(z => z.StartDate).Take(2)).ThenInclude(z => z.AppCost)
                                                .FirstOrDefaultAsync(x => x.Id.ToString() == request.PatientId);

            if (patient == null)
            {
                throw new CustomMessageException("No patient found to add contract");
            }

            if (patient.Company != null)
            {
                throw new CustomMessageException("Patient is part of a company");
            }

            var hasContract = patient.PatientContracts.FirstOrDefault();

            if (hasContract != null && hasContract.StartDate.AddMonths(hasContract.Duration) > DateTime.Now && hasContract.AppCost.PaymentStatus != Models.Enums.PaymentStatus.canceled)
            {
                throw new CustomMessageException("Patient already has a contract");
            }

            var Description = "First Registration";

            if (hasContract != null)
            {
                Description = "Renew Contract";
            }

            var newContract = new PatientContract
            {
                PatientId = patient.Id,
                StartDate = DateTime.Today.AddDays(1),
                Duration = request.DurationInMonths.Value,
            };

            AppCost appCost = CreateCost(cost, Description);

            newContract.AppCostId = appCost.Id;

            await iDBRepository.AddAsync<AppCost>(appCost);
            await iDBRepository.AddAsync<PatientContract>(newContract);

            await iDBRepository.Complete();

            return Unit.Value;
        }

        private static AppCost CreateCost(int cost, string Description)
        {
            var appCost = new AppCost
            {
                Id = Guid.NewGuid(),
                Amount = cost,
                ApprovedPrice = cost,
                CostType = Models.Enums.AppCostType.profit,
                Description = Description
            };

            var financialRecord = new FinancialRecord
            {

            };

            return appCost;
        }
    }
}
