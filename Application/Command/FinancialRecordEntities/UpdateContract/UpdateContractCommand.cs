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
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.FinancialRecordEntities.UpdateContract
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
        private ICompanyRepository _companyRepository { get; set; }

        public UpdateContractHandler(IFinancialRespository financialRespository, IDBRepository dBRepository, ICompanyRepository companyRepository)
        {
            _financialRespository = financialRespository;
            _dBRepository = dBRepository;
            _companyRepository = companyRepository;
        }

        public async Task<Unit> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            var homeCompany = await _companyRepository.Companies()
                                    .FirstOrDefaultAsync(x => x.HomeCompany);

            if (homeCompany == null)
            {
                throw new CustomMessageException("Home Company is required");
            }

            var tax = await _financialRespository.GetTax();

            var appCost = await _financialRespository.GetAppCosts()
                                .Include(x => x.FinancialRecordPayerPayees)
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
            var financialRecord = FinancialHelper.AppCostToFinancialRecord(appCost);

            foreach (var item in financialRecord.FinancialRecordPayerPayees)
            {
                await _dBRepository.AddAsync<FinancialRecordPayerPayee>(item);
            }

            appCost.FinancialRecordId = financialRecord.Id;


            financialRecord.Payments = appCost.Payments;
            financialRecord.ActorId = request.getCurrentUserRequest().CurrentUser.Id;

            if (appCost.PatientContract != null)
            {
                appCost.PatientContract.StartDate = DateTime.Today.AddDays(1).ToUniversalTime();
                _dBRepository.Update(appCost.PatientContract);
            }

            if (appCost.CompanyContract != null)
            {
                appCost.CompanyContract.StartDate = DateTime.Today.AddDays(1).ToUniversalTime();
                _dBRepository.Update(appCost.CompanyContract);
            }

            await _dBRepository.AddAsync(financialRecord);

            _dBRepository.Update(appCost);

            await _dBRepository.Complete();

            return Unit.Value;
        }
    }
}
