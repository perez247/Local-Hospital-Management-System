using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.FinancialRecordEntities.GetPendingUserContracts
{
    public class GetPendingUserContractsFilterValidator : AbstractValidator<GetPendingUserContractsFilter>
    {
        public GetPendingUserContractsFilterValidator()
        {
            RuleFor(x => x.PaymentStatus)
                .Must(x => CommonValidators.EnumsContains<PaymentStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(PaymentStatus)))}")
                .When(x => !string.IsNullOrEmpty(x.PaymentStatus));
        }
    }
}
