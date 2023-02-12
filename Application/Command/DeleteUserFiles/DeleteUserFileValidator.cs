using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.DeleteUserFiles
{
    public class DeleteUserFileValidator : AbstractValidator<DeleteUserFIlesCommand>
    {
        public DeleteUserFileValidator()
        {
            RuleFor(x => x.UserFileIds)
                .Must(x => x != null && x.Count() > 0).WithMessage("Files to delete is required");
        }
    }
}
