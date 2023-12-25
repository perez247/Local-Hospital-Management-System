using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.ActivityLogEntities.GetActivityLog
{
    public class GetActivityLogValidator: AbstractValidator<GetActivityLogQuery>
    {
        public GetActivityLogValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new GetActivityLogQueryValidator());
        }
    }
}
