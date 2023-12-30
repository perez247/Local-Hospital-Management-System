using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.StaffEntities.UpdateStaffDetails
{
    public class UpdateStaffDetailValidator : AbstractValidator<UpdateStaffDetailCommand>
    {
        public UpdateStaffDetailValidator()
        {
            RuleFor(x => x.Level)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Level is requires")
                .MaximumLength(200).WithMessage("Maximum of 200 chars");

            RuleFor(x => x.Salary)
                .Must(x => x.HasValue).WithMessage("Salary is requires");

            RuleFor(x => x.Position)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Position is requires")
                .MaximumLength(200).WithMessage("Maximum of 200 chars");

            RuleFor(x => x.AccountNumber)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Account Number is requires")
                .MaximumLength(200).WithMessage("Maximum of 200 chars");

            RuleFor(x => x.BankName)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Bank Name is requires")
                .MaximumLength(200).WithMessage("Maximum of 200 chars");

            RuleFor(x => x.BankId)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Bank Id is requires")
                .MaximumLength(200).WithMessage("Maximum of 200 chars");
        }
    }
}
