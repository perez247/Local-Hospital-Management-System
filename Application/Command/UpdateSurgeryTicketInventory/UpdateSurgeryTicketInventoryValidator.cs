using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateSurgeryTicketInventory
{
    public class UpdateSurgeryTicketInventoryValidator : AbstractValidator<UpdateSurgeryTicketInventoryCommand>
    {
        public UpdateSurgeryTicketInventoryValidator()
        {
            RuleFor(x => x.UpdateSurgeryTicketInventoryRequest)
                .Must(x => x != null && x.Count > 0);

            RuleForEach(x => x.UpdateSurgeryTicketInventoryRequest)
                .SetValidator(new UpdateSurgeryTicketInventoryRequestValidator())
                .When(x => x.UpdateSurgeryTicketInventoryRequest != null && x.UpdateSurgeryTicketInventoryRequest.Count > 0);
        }
    }
}
