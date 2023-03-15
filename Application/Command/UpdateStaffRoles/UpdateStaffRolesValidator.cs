using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateStaffRoles
{
    public class UpdateStaffRolesValidator : AbstractValidator<UpdateStaffRolesCommand>
    {
        public UpdateStaffRolesValidator()
        {
            RuleFor(x => x.StaffRoleEnum)
                .Must(x => x != null && x.Count() > 0).WithMessage("At least one role is required");

            RuleForEach(x => x.StaffRoleEnum)
                .Must(x => CommonValidators.EnumsContains<StaffRoleEnum>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(StaffRoleEnum)))}");
        }
    }
}
