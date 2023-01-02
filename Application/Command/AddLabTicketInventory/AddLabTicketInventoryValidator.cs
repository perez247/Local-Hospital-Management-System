using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddLabTicketInventory
{
    public class AddLabTicketInventoryValidator : AbstractValidator<AddLabTicketInventoryCommand>
    {
        public AddLabTicketInventoryValidator()
        {
            RuleFor(x => x.AddLabTicketInventoryRequest)
                .Must(x => x != null && x.Count > 0).WithMessage("Request is required");

            RuleForEach(x => x.AddLabTicketInventoryRequest)
                .SetValidator(new AddLabTicketInventoryRequestValidator());
        }
    }
}
