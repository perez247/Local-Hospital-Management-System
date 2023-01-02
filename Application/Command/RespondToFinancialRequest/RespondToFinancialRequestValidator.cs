using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.RespondToFinancialRequest
{
    public class RespondToFinancialRequestValidator : AbstractValidator<RespondToFinancialRequestCommand>
    {
        public RespondToFinancialRequestValidator()
        {
            RuleFor(x => x.PaymentStatus)
                .Must(x => CommonValidators.EnumsContains<PaymentStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(PaymentStatus)))}");

            RuleFor(x => x.PaymentType)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Payment status is required")
                .Must(x => CommonValidators.EnumsContains<PaymentType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(PaymentType)))}");

            RuleFor(x => x.Proof)
                .Must(CommonValidators.IsBase64String).WithMessage("Invalid base 64 string")
                .MaximumLength(15000).WithMessage("Maximum of 15000 chars")
                .When(x => !string.IsNullOrEmpty(x.Proof));

            RuleFor(x => x.PaymentDetails)
                .MaximumLength(5000).WithMessage("Maximum of 5000 chars")
                .When(x => !string.IsNullOrEmpty(x.PaymentDetails));

            RuleFor(x => x.Description)
                .MaximumLength(5000).WithMessage("Maximum of 5000 chars")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
