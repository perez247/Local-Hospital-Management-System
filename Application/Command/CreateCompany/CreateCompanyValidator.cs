using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.CreateCompany
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._]*$").WithMessage("Only letters, numbers, periods and underscore");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is invalid");
            //.MustAsync(async (x, y, z) => await _auth.IsEmailAvailable(x.Email)).WithMessage("E-mail belongs to another user");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");

            RuleFor(x => x.UniqueId)
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");

            RuleFor(x => x.OtherId)
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");
        }
    }
}
