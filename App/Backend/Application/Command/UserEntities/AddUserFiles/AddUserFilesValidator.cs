using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UserEntities.AddUserFiles
{
    public class AddUserFilesValidator : AbstractValidator<AddUserFilesCommand>
    {
        public AddUserFilesValidator()
        {
            RuleFor(x => x.UserFiles)
                .Must(x => x != null && x.Count > 0).WithMessage("Files are required")
                .Must(x => x != null && x.Count <= 5).WithMessage("You can only upload 5 files at a time");

            RuleForEach(x => x.UserFiles)
                .SetValidator(new AddUserFilesRequestValidator());
        }
    }
}
