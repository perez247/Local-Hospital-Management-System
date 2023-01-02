using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddLabTicketInventory
{
    public class AddLabTicketInventoryRequestValidator : AbstractValidator<AddLabTicketInventoryRequest>
    {
        public AddLabTicketInventoryRequestValidator()
        {
            RuleFor(x => x.PrescribedLabRadiologyFeature)
                .MaximumLength(1000).WithMessage("Maximum of 1000 chars");
        }
    }
}
