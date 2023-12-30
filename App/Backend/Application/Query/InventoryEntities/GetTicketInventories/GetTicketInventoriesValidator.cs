using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetTicketInventories
{
    public class GetTicketInventoriesValidator: AbstractValidator<GetTicketInventoriesQuery>
    {
        public GetTicketInventoriesValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new GetTicketInventoriesFilterValidator());
        }
    }
}
