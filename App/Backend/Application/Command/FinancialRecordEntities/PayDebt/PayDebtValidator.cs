using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.FinancialRecordEntities.PayDebt
{
    public class PayDebtValidator: AbstractValidator<PayDebtCommand>
    {
        public PayDebtValidator()
        {
            RuleFor(x => x.AmountToPay)
                .Must(x => x.HasValue).WithMessage("Amount to pay is required");

            RuleFor(x => x.Proof)
                .Must(x => CommonValidators.IsBase64String(x)).WithMessage("Invalid file uploaded");

            RuleFor(x => x.PaymentType)
                .Must(x => CommonValidators.EnumsContains<PaymentType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(PaymentType)))}");
        }
    }
}
