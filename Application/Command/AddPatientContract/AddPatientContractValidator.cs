using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddPatientContract
{
    public class AddPatientContractValidator : AbstractValidator<AddPatientContractCommand>
    {
        public AddPatientContractValidator()
        {
            RuleFor(x => x.DurationInMonths)
                .Must(x => x.HasValue).WithMessage("Duration is required")
                .GreaterThan(0).WithMessage("Must be at least 1 month")
                .LessThanOrEqualTo(12).WithMessage("Must be less than or equal to 12");
        }
    }
}
