using Application.Validations;
using FluentValidation;
using Models.Enums;
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

            RuleFor(x => x.AppTicketStatus)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Status is required")
                .Must(x => CommonValidators.EnumsContains<AppTicketStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppTicketStatus)))}");

            RuleFor(x => x.PrescribedQuantity)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Quantity Given is required")
                .Must(x => Int32.TryParse(x, out int result)).WithMessage("Must be a number")
                .MaximumLength(1000);

            RuleFor(x => x.DepartmentDescription)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.DepartmentDescription));

            RuleFor(x => x.StaffObservation)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.StaffObservation));

            RuleFor(x => x.AdditionalNote)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrEmpty(x.DepartmentDescription));
        }
    }
}
