using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.PatientEntities.AddPatientContract
{
    public class AddPatientContractValidator : AbstractValidator<AddPatientContractCommand>
    {
        public AddPatientContractValidator()
        {
            RuleFor(x => x.DurationInDays)
                .Must(x => x.HasValue).WithMessage("Duration is required")
                .GreaterThanOrEqualTo(365).WithMessage("Must be at least 1 Year")
                .LessThanOrEqualTo(1828).WithMessage("Must be less than or equal to 5 years");

            RuleFor(x => x.Amount)
            .Must(x => x.HasValue).WithMessage("Amount is required")
            .GreaterThanOrEqualTo(1000).WithMessage("Amount must be at least 1000");
        }
    }
}
