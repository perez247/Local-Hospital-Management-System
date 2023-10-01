using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AdmissionEntities.ExecutePrescription
{
    public class ExecutePrescriptionValidator: AbstractValidator<ExecutePrescriptionCommand>
    {
        public ExecutePrescriptionValidator()
        {
            RuleFor(x => x.TimeGiven)
                .Must(x => x.HasValue).WithMessage("Time given is required");
        }
    }
}
