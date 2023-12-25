using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.PatientEntities.UpdatePatientAllergy
{
    public class UpdatePatientAllergiesValidator : AbstractValidator<UpdatePatientAllergyCommand>
    {
        public UpdatePatientAllergiesValidator()
        {
            RuleFor(x => x.Allergies)
                .MaximumLength(10000).WithMessage("Maximum of 100000");
        }
    }
}
