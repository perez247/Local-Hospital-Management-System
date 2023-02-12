using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetUserList
{
    public class GetUserListFilterValidator : AbstractValidator<GetUserListFilter>
    {
        public GetUserListFilterValidator()
        {

            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._]*$").WithMessage("Only letters, numbers, periods and underscore")
                .When(x => string.IsNullOrEmpty(x.Name));
        }
    }
}
