using Application.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.ConcludeSurgeryTicket
{
    public class ConcludeSurgeryTicketRequestValidator : AbstractValidator<ConcludeSurgeryTicketRequest>
    {
        public ConcludeSurgeryTicketRequestValidator()
        {
            Include(new ConcludeTicketRequestValidator());

            RuleFor(x => x.SurgeryTestResult)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Surgery result is required");

            RuleForEach(x => x.ItemsUsed)
                .Must(x => x.Quantity > 0).WithMessage("Quantity can not be lest than or equal to zero")
                .When(x => x.ItemsUsed != null && x.ItemsUsed.Count() > 0);
        }
    }
}
