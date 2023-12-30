using Application.Command.TicketEntities.ConcludeLabRadTicket;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.ConcludeSurgeryTicket
{
    public class ConcludeSurgeryTicketValidator : AbstractValidator<ConcludeSurgeryTicketCommand>
    {
        public ConcludeSurgeryTicketValidator()
        {
            RuleFor(x => x.ConcludeTicketRequest)
                .Must(x => x != null && x.Count() > 0).WithMessage("At least one Inventory is required");

            RuleForEach(x => x.ConcludeTicketRequest)
                .SetValidator(new ConcludeSurgeryTicketRequestValidator());
        }
    }
}
