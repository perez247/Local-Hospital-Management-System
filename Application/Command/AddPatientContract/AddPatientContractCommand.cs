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

        public AddPatientContractHandler(IPatientRepository IPatientRepository, IDBRepository IDBRepository)
        {
            iPatientRepository = IPatientRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(AddPatientContractCommand request, CancellationToken cancellationToken)
        {
            // TODO: - Create a setting table to store cost for registration for a patient
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

            AppCost appCost = CreateCost(cost, Description);

            newContract.AppCostId = appCost.Id;

            await iDBRepository.AddAsync<AppCost>(appCost);
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

        private static AppCost CreateCost(int cost, string Description)
        {
            var appCost = new AppCost
            {
                Id = Guid.NewGuid(),
                Amount = cost,
                ApprovedPrice = cost,
                CostType = Models.Enums.AppCostType.profit,
                Description = Description,
            };

            var financialRecord = new FinancialRecord
            {

            };

            return appCost;
        }
    }
}
