using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.UpdatePharmacyTicketInventory
{
    public class UpdatePharmacyTicketInventoryValidator : AbstractValidator<UpdatePharmacyTicketInventoryCommand>
    {
        public UpdatePharmacyTicketInventoryValidator()
        {
            RuleFor(x => x.UpdatePharmacyTicketInventoryRequest)
                .Must(x => x != null && x.Count() > 0);

            RuleForEach(x => x.UpdatePharmacyTicketInventoryRequest)
                .SetValidator(x => new UpdatePharmacyTicketInventoryRequestValidator())
                .When(x => x.UpdatePharmacyTicketInventoryRequest != null && x.UpdatePharmacyTicketInventoryRequest.Count() > 0);
        }
    }
}
