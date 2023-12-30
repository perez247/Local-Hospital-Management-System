using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.FinancialRecordEntities.UpdateContract
{
    public class UpdateContractValidator : AbstractValidator<UpdateContractCommand>
    {
        public UpdateContractValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._ ]*$").WithMessage("Only letters, numbers, periods and underscore");

            RuleFor(x => x.Base64String)
                .Must(x => CommonValidators.IsBase64String(x)).WithMessage("Invalid Proof uploaded");

            RuleFor(x => x.Amount)
                .Must(x => x.HasValue).WithMessage("Amount is required")
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal 0");

            RuleFor(x => x.PaymentType)
                .Must(x => CommonValidators.EnumsContains<PaymentType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(PaymentType)))}");
        }
    }
}
