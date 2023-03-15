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

namespace Application.Command.UpdateContract
{
    public class UpdateContractCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? AppCostId { get; set; }
        public string? Name { get; set; }
        public string? Base64String { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentType { get; set; }
    }

    public class UpdateContractHandler : IRequestHandler<UpdateContractCommand, Unit>
    {
        private readonly IFinancialRespository _financialRespository;
        private readonly IDBRepository _dBRepository;

        public UpdateContractHandler(IFinancialRespository financialRespository, IDBRepository dBRepository)
        {
            _financialRespository = financialRespository;
            _dBRepository = dBRepository;
        }

        public async Task<Unit> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            var tax = await _financialRespository.GetTax();

            var appCost = await _financialRespository.GetAppCosts()
                                .Include(x => x.FinancialRecord)
                                .Include(x => x.PatientContract)
                                .Include(x => x.CompanyContract)
                                .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppCostId);

            if (appCost == null)
            {
                throw new CustomMessageException("Contract not found");
            }

            if (appCost.Amount != request.Amount.Value)
            {
                throw new CustomMessageException($"Amount must be {appCost.Amount}");
            }

            appCost.Payments = new List<Payment>()
            {
                new Payment() {
                    Amount = Math.Round(request.Amount.Value, 2),
                    Tax = Math.Round(request.Amount.Value, 2) * tax,
                    PaymentType = request.PaymentType.ParseEnum<PaymentType>(),
                    Proof = request.Base64String,
                    DatePaid = DateTime.Now.ToUniversalTime(),
                },
            };

            appCost.PaymentStatus = PaymentStatus.approved;

            var financialRecord = appCost.FinancialRecord;

            financialRecord.Payments = appCost.Payments;
            financialRecord.PaymentStatus = PaymentStatus.approved;

            if (appCost.PatientContract != null)
            {
                appCost.PatientContract.StartDate = DateTime.Today.AddDays(1).ToUniversalTime();
                _dBRepository.Update<PatientContract>(appCost.PatientContract);
            }

            if (appCost.CompanyContract != null)
            {
                appCost.CompanyContract.StartDate = DateTime.Today.AddDays(1).ToUniversalTime();
                _dBRepository.Update<CompanyContract>(appCost.CompanyContract);
            }

            _dBRepository.Update<FinancialRecord>(financialRecord);
            _dBRepository.Update<AppCost>(appCost);

            await _dBRepository.Complete();

            return Unit.Value;
        }
    }
}
