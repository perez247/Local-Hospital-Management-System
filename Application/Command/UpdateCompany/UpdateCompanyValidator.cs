using Application.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateCompany
{
    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._ ]*$").WithMessage("Only letters, numbers, periods and underscore");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");

            RuleFor(x => x.UniqueId)
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");

            RuleFor(x => x.OtherId)
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");

            RuleFor(x => x.Profile)
                .Must(x => CommonValidators.IsBase64String(x)).WithMessage("Invalid Image")
                .When(x => !string.IsNullOrEmpty(x.Profile));
        }
    }
}
