using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddPatientVital
{
    public class AddPatientVitalValidator : AbstractValidator<AddPatientVitalCommand>
    {
        public AddPatientVitalValidator()
        {
            RuleFor(x => x.Data)
                .MaximumLength(5000).WithMessage("Not more than 5000 chars");
        }
    }
}
