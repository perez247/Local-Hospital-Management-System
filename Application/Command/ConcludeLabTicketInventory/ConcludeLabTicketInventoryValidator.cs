using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.ConcludeLabTicketInventory
{
    public class ConcludeLabTicketInventoryValidator : AbstractValidator<ConcludeLabTicketInventoryCommand>
    {
        public ConcludeLabTicketInventoryValidator()
        {
            RuleFor(x => x.ConcludeLabTicketInventoryRequest)
                .Must(x => x!= null && x.Count > 0).WithMessage("Request is required");

            RuleForEach(x => x.ConcludeLabTicketInventoryRequest)
                .SetValidator(new ConcludeLabTicketInventoryRequestValidator());
        }
    }
}
