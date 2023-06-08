using Application.DTOs;
using Application.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.SendLabTicketsToFinance
{
    public class SendLabTicketsToFinanceValidator : AbstractValidator<SendLabTicketsToFinanceCommand>
    {
        public SendLabTicketsToFinanceValidator()
        {
            RuleFor(x => x.TicketInventories)
                .Must(x => x != null && x.Count() > 0).WithMessage("At least one ticket inventory is required");

            RuleForEach(x => x.TicketInventories)
                .SetValidator(new SendTicketToFinanceRequestValidator());
        }
    }
}
