using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.TicketEntities.GetTickets
{
    public class GetTicketQueryValidator: AbstractValidator<GetTicketsQuery>
    {
        public GetTicketQueryValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new GetTicketFilterValidator());
        }
    }
}
