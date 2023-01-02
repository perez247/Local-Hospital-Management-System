using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.CreateFinancialRequest
{
    public class CreateFinancialRequestValidator : AbstractValidator<CreateFinancialRequestCommand>
    {
        public CreateFinancialRequestValidator()
        {
            RuleFor(x => x.AppCostType)
                .Must(x => CommonValidators.EnumsContains<AppCostType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppCostType)))}");

            RuleFor(x => x.Description)
                .MaximumLength(5000).WithMessage("Maximum of 5000 chars");

            RuleFor(x => x.Amount)
                .Must(x => x.HasValue)
                .GreaterThanOrEqualTo(1).WithMessage("Must be greater than 1");
        }
    }
}
