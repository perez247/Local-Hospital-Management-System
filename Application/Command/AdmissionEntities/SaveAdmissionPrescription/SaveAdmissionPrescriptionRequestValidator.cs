using Application.Utilities;
using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AdmissionEntities.SaveAdmissionPrescription
{
    public class SaveAdmissionPrescriptionRequestValidator : AbstractValidator<SaveAdmissionPrescriptionRequest>
    {
        public SaveAdmissionPrescriptionRequestValidator()
        {
            RuleFor(x => x.DoctorsPrescription)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.DoctorsPrescription));

            RuleFor(x => x.PrescribedQuantity)
                .MaximumLength(1000)
                .Must(x => Int32.TryParse(x, out int result)).WithMessage("Must be a number")
                .When(x => !string.IsNullOrEmpty(x.PrescribedQuantity));

            RuleFor(x => x.AppInventoryType)
                .Must(x => CommonValidators.EnumsContains<AppInventoryType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppInventoryType)))}");

            RuleFor(x => x.Times)
                .Must(x => x.HasValue).WithMessage("Times is required")
                .GreaterThanOrEqualTo(1).WithMessage("Must be greater than 1");
                //.When(x => x.AppInventoryType.ParseEnum<AppInventoryType>() == AppInventoryType.pharmacy);

            RuleFor(x => x.Dosage)
                .Must(x => x.HasValue).WithMessage("Dosage is required")
                .GreaterThanOrEqualTo(1).WithMessage("Must be greater than 1");

            RuleFor(x => x.Frequency)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Frequency is required");

            RuleFor(x => x.Duration)
                .Must(x => x.HasValue).WithMessage("Duration is required")
                .GreaterThanOrEqualTo(1).WithMessage("Must be greater than 1")
                .LessThanOrEqualTo(30).WithMessage("Must be less than 31");
        }
    }
}
