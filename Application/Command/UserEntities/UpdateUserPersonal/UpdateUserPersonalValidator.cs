using Application.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UserEntities.UpdateUserPersonal
{
    public class UpdateUserPersonalValidator : AbstractValidator<UpdateUserPersonalCommand>
    {
        public UpdateUserPersonalValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._]*$").WithMessage("Only letters, numbers, periods and underscore");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._]*$").WithMessage("Only letters, numbers, periods and underscore");

            RuleFor(x => x.OtherName)
                .NotEmpty().WithMessage("Other Name is required")
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._]*$").WithMessage("Only letters, numbers, periods and underscore")
                .When(x => !string.IsNullOrEmpty(x.OtherName));

            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required").MaximumLength(100).WithMessage("Maximum of 100 chars")
                .Matches("^[0-9+]*$").WithMessage("Only numbers and +");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");

            RuleFor(x => x.Email)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid Email Address");

            RuleFor(x => x.CompanyUniqueId)
                .MaximumLength(255).WithMessage("Maximum of 255 chars")
                .When(x => !string.IsNullOrEmpty(x.CompanyUniqueId));

            RuleFor(x => x.OtherInformation)
                .MaximumLength(2000).WithMessage("Maximum of 1000 chars")
                .When(x => !string.IsNullOrEmpty(x.OtherInformation));

            RuleFor(x => x.Profile)
                .Must(x => CommonValidators.IsBase64String(x)).WithMessage("Invalid Image")
                .When(x => !string.IsNullOrEmpty(x.Profile));

            RuleFor(x => x.Occupation)
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .When(x => !string.IsNullOrEmpty(x.Occupation));

            RuleFor(x => x.Gender)
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .When(x => !string.IsNullOrEmpty(x.Gender));
        }
    }
}
