using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.PatientEntities.PatientVitals
{
    public class PatientVitalValidator : AbstractValidator<PatientVitalsQuery>
    {
        public PatientVitalValidator()
        {
            RuleFor(x => x.Filter)
                .Must(x => x != null).WithMessage("Filter is required");
        }
    }
}
