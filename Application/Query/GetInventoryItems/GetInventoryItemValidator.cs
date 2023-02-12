using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetInventoryItems
{
    public class GetInventoryItemValidator : AbstractValidator<GetInventoryItemsQuery>
    {
        public GetInventoryItemValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new GetInventoryItemFilterValidator())
                .When(x => x.Filter != null);
        }
    }
}
