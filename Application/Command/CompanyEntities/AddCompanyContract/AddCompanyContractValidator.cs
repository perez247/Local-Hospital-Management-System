using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.CompanyEntities.AddCompanyContract
{
    public class AddCompanyContractValidator : AbstractValidator<AddCompanyContractCommand>
    {
        public AddCompanyContractValidator()
        {
            RuleFor(x => x.DurationInDays)
                .Must(x => x.HasValue).WithMessage("Duration is required")
                .GreaterThan(28).WithMessage("Must be at least 1 Month")
                .LessThanOrEqualTo(366).WithMessage("Must be less than or equal to 12 months");
        }
    }
}
