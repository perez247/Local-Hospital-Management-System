using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utilities;
using MediatR;
using Application.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;
using Microsoft.VisualBasic;
using Application.Annotations;

namespace Application.Command.FinancialRecordEntities.SaveRecord
{
    public class SaveRecordCommand: TokenCredentials, IRequest<Unit>
    {
        public decimal? Amount { get; set; }
        public string? AppCostType { get; set; }

        [VerifyGuidAnnotation]
        public string? ActeeId { get; set; }
        public string? Description { get; set; }
    }

    public class SaveRecordHandler : IRequestHandler<SaveRecordCommand, Unit>
    {
        private IDBRepository _dBRepository { get; set; }
        private ICompanyRepository _companyRepository { get; set; }
        private IUserRepository _userRepository { get; set; }
        public SaveRecordHandler(IDBRepository dBRepository, ICompanyRepository companyRepository, IUserRepository userRepository)
        {
            _dBRepository = dBRepository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(SaveRecordCommand request, CancellationToken cancellationToken)
        {
            var homeCompany = await _companyRepository.Companies()
                                    .FirstOrDefaultAsync(x => x.HomeCompany);

            if (homeCompany == null)
            {
                throw new CustomMessageException("Home Company is required");
            }

            var actee = await _userRepository.Users().FirstOrDefaultAsync(s => s.Id.ToString() == request.ActeeId);

            if (actee == null)
            {
                throw new CustomMessageException("User to recieve or make payment not found");
            }

            var costType = request.AppCostType.ParseEnum<AppCostType>();
            var FinancialRecordPayerPayees = new List<FinancialRecordPayerPayee>
            {
                new FinancialRecordPayerPayee
                {
                    AppUserId = actee.Id,
                    Payer = costType == AppCostType.profit,
                },                
                new FinancialRecordPayerPayee
                {
                    AppUserId = homeCompany.AppUserId,
                    Payer = costType == AppCostType.expense,
                }
            };



            var newFinancialRecord = new FinancialRecord
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                ApprovedAmount = request.Amount,
                PaymentStatus = PaymentStatus.approved,
                CostType = costType,
                Description = request.Description,
                ManualEntry = true,
                ActorId = request.getCurrentUserRequest().CurrentUser.Id,
                FinancialRecordPayerPayees = FinancialRecordPayerPayees
            };

            await _dBRepository.AddAsync<FinancialRecord>(newFinancialRecord);
            await _dBRepository.Complete();

            return Unit.Value;
        }
    }
}
