using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateUserNextofKin
{
    public class UpdateUserNextofKinValidator : AbstractValidator<UpdateUserNextofKinCommand>
    {
        public UpdateUserNextofKinValidator()
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

            RuleFor(x => x.Phone1).NotEmpty().WithMessage("Phone1 is required").MaximumLength(100).WithMessage("Maximum of 100 chars")
                .Matches("^[0-9+]*$").WithMessage("Only numbers and +");

            RuleFor(x => x.Phone2).NotEmpty().WithMessage("Phone2 is required").MaximumLength(100).WithMessage("Maximum of 100 chars")
                .Matches("^[0-9+]*$").WithMessage("Only numbers and +")
                .When(x => !string.IsNullOrEmpty(x.Phone2));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is invalid")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");
        }
    }
}
