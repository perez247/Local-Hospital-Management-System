using FluentValidation;
using Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.UserEntities.GetUserList
{
    public class GetUserListFilterValidator : AbstractValidator<GetUserListFilter>
    {
        public GetUserListFilterValidator()
        {

            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._]*$").WithMessage("Only letters, numbers, periods and underscore")
                .When(x => string.IsNullOrEmpty(x.Name));

            RuleForEach(x => x.Roles)
                .Must(x => ApplicationRoles.Roles().Contains(x.ToLower())).WithMessage("Invalid role selected")
                .When(x => x.Roles != null && x.Roles.Count > 0);
        }
    }
}
