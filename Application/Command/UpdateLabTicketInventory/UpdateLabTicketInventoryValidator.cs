using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateLabTicketInventory
{
    public class UpdateLabTicketInventoryValidator : AbstractValidator<UpdateLabTicketInventoryCommand>
    {
        public UpdateLabTicketInventoryValidator()
        {
            RuleFor(x => x.UpdateLabTicketInventoryRequest)
                .Must(x => x != null && x.Count > 0).WithMessage("Request is required");

            RuleForEach(x => x.UpdateLabTicketInventoryRequest)
                .SetValidator(new UpdateLabTicketInventoryRequestValidator());
        }
    }
}
