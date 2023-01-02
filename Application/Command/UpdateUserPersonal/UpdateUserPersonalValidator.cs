using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateUserPersonal
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
        }
    }
}
