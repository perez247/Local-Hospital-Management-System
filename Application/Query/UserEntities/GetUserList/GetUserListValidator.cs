using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.UserEntities.GetUserList
{
    public class GetUserListValidator : AbstractValidator<GetUserListQuery>
    {
        public GetUserListValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new GetUserListFilterValidator())
                .When(x => x.Filter != null);
        }
    }
}
