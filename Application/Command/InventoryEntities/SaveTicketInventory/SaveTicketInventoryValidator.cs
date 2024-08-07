﻿using Application.Command.TicketEntities.UpdateSurgeryTicket;
using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.SaveTicketInventory
{
    public class SaveTicketInventoryValidator : AbstractValidator<SaveTicketInventoryCommand>
    {
        public SaveTicketInventoryValidator()
        {
            RuleFor(x => x.AppTicketStatus)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Status is required")
                .Must(x => CommonValidators.EnumsContains<AppTicketStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppTicketStatus)))}");

            RuleFor(x => x.Times)
                .Must(x => x.HasValue).WithMessage("Times is required")
                .GreaterThanOrEqualTo(1).WithMessage("Must be greater than 1");

            RuleFor(x => x.Dosage)
                .Must(x => x.HasValue).WithMessage("Dosage is required")
                .GreaterThanOrEqualTo(1).WithMessage("Must be greater than 1");

            RuleFor(x => x.Duration)
                .Must(x => x.HasValue).WithMessage("Duration is required")
                .GreaterThanOrEqualTo(1).WithMessage("Must be greater than 1")
                .LessThanOrEqualTo(30).WithMessage("Must be less than 31");

            RuleFor(x => x.Frequency)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Frequency is required");
            
            RuleFor(x => x.DoctorsPrescription)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.DoctorsPrescription));

            RuleFor(x => x.PrescribedQuantity)
                .MaximumLength(1000)
                .Must(x => Int32.TryParse(x, out int result)).WithMessage("Must be a number")
                .When(x => !string.IsNullOrEmpty(x.PrescribedQuantity));

            RuleFor(x => x.DepartmentDescription)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.DepartmentDescription));

            RuleFor(x => x.AdditionalNote)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrEmpty(x.PrescribedQuantity));


            RuleFor(x => x.DepartmentDescription)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.DepartmentDescription));

            RuleFor(x => x.StaffObservation)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.StaffObservation));

            RuleFor(x => x.ConcludedPrice)
                .Must(x => x.HasValue && x.Value > 0).WithMessage("Concluded price must be greater than or equal to zero")
                .Must(x => x.HasValue).WithMessage("Concluded Price is required");

            RuleForEach(x => x.Proof)
                .Must(x => CommonValidators.IsBase64String(x)).WithMessage("Invalid Base 64 String")
                .When(x => x.Proof != null && x.Proof.Count > 0);

            RuleFor(x => x.labRadiologyTestResult)
                .MaximumLength(5000).WithMessage("Not more than 5000 characters")
                .When(x => !string.IsNullOrEmpty(x.labRadiologyTestResult));

            RuleFor(x => x.SurgeryTestResult)
                .MaximumLength(5000).WithMessage("Not more than 5000 characters")
                .When(x => !string.IsNullOrEmpty(x.SurgeryTestResult));

            RuleFor(x => x.AdmissionEndDate)
                .Must((x, y, z) => x.AdmissionStartDate.HasValue).WithMessage("Admission Start Date is required")
                .Must((x, y, z) => x.AdmissionEndDate > x.AdmissionStartDate).WithMessage("Admission Start date must be less than end date")
                .When(x => x.AdmissionEndDate.HasValue);

            RuleForEach(x => x.SurgeryTicketPersonnels)
                .SetValidator(new UpdateSurgeryTicketPersonnelValidator())
                .When(x => x.SurgeryTicketPersonnels != null && x.SurgeryTicketPersonnels.Count > 0);

            RuleForEach(x => x.Debtors)
                .SetValidator(new SaveTicketInventoryDebtorValidator())
                .When(x => x.Debtors != null && x.Debtors.Count > 0);

        }
    }
}
