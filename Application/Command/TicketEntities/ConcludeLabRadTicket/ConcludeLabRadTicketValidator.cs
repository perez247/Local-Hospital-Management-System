using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.ConcludeLabRadTicket
{
    public class ConcludeLabRadTicketValidator : AbstractValidator<ConcludeLabRadTicketCommand>
    {
        public ConcludeLabRadTicketValidator()
        {
            RuleFor(x => x.ConcludeTicketRequest)
                .Must(x => x != null && x.Count() > 0).WithMessage("At least one Inventory is required");

            RuleForEach(x => x.ConcludeTicketRequest)
                .SetValidator(new ConcludeLabRadTicketRequestValidator());
        }
    }
}
