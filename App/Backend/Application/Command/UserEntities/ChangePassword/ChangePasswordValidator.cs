using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UserEntities.ChangePassword
{
    public class ChangePasswordValidator: AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Current Password is required")
                .MinimumLength(6).WithMessage("Minimum of 6 chars")
                .MaximumLength(50).WithMessage("Minimum of 6 chars");

            RuleFor(x => x.NewPassword)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("New Password is required")
                .MinimumLength(6).WithMessage("Minimum of 6 chars")
                .MaximumLength(50).WithMessage("Minimum of 6 chars");
        }
    }
}
