using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.AdmissionEntities.GetPrescriptions
{
    public class GetPrescriptionQueryFilterValidator : AbstractValidator<GetPrescriptionQueryFilter>
    {
        public GetPrescriptionQueryFilterValidator()
        {
            RuleFor(x => x.AppInventoryType)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Type is required")
                .Must(x => CommonValidators.EnumsContains<AppInventoryType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppInventoryType)))}");
        }
    }
}
