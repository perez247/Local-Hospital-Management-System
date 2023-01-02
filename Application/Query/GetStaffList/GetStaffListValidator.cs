using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetStaffList
{
    public class GetStaffListValidator : AbstractValidator<GetStaffListQuery>
    {
        public GetStaffListValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new GetStaffListFilterValidator())
                .When(x => x.Filter != null);
        }
    }
}
