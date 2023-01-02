using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddPharmacyTicketInventory
{
    public class AddPharmacyTicketValidator : AbstractValidator<AddPharmacyTicketInventoryCommand>
    {
        public AddPharmacyTicketValidator()
        {
            RuleFor(x => x.TicketInventories)
                .Must(x => x != null && x.Count > 0).WithMessage("Inventory is required");

            RuleForEach(x => x.TicketInventories)
                .SetValidator(x => new AddPharmacyTicketRequestValidator())
                .When(x => x != null && x.TicketInventories.Count > 0);
        }
    }
}
