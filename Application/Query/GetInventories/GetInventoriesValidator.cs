using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetInventories
{
    public class GetInventoriesValidator: AbstractValidator<GetInventoriesQuery>
    {
        public GetInventoriesValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new GetInventoriesFilterValidator())
                .When(x => x.Filter != null);
        }
    }
}
