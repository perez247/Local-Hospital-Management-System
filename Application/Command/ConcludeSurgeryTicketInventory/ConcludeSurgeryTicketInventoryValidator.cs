using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.ConcludeSurgeryTicketInventory
{
    public class ConcludeSurgeryTicketInventoryValidator : AbstractValidator<ConcludeSurgeryTicketInventoryCommand>
    {
        public ConcludeSurgeryTicketInventoryValidator()
        {
            RuleFor(x => x.ConcludeSurgeryTicketInventoryRequest)
                .Must(x => x != null && x.Count() > 0).WithMessage("Inventories are required");

            RuleForEach(x => x.ConcludeSurgeryTicketInventoryRequest)
                .SetValidator(new ConcludeSurgeryTicketInventoryRequestValidator());
        }
    }
}
