using Application.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddUserFiles
{
    public class AddUserFilesRequestValidator : AbstractValidator<AddUserFilesRequest>
    {
        public AddUserFilesRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._ ]*$").WithMessage("Only letters, numbers, periods and underscore");

            RuleFor(x => x.Base64String)
                .NotEmpty().WithMessage("File is required")
                .Must(x => CommonValidators.IsBase64String(x)).WithMessage("Invalid Base 64 string");
        }
    }
}
