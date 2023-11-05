using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.AdmissionEntities.GetPrescriptions
{
    public class GetPrescriptionQueryValidator : AbstractValidator<GetPrescriptionsQuery>
    {
        public GetPrescriptionQueryValidator()
        {
            RuleFor(x => x.Filter)
                .Must(x => x != null).WithMessage("Filter is required")
                .SetValidator(new GetPrescriptionQueryFilterValidator());
        }
    }
}
