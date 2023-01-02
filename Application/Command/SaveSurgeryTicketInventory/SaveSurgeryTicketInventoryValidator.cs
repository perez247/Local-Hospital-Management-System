using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.SaveSurgeryTicketInventory
{
    public class SaveSurgeryTicketInventoryValidator : AbstractValidator<SaveSurgeryTicketInventoryCommand>
    {
        public SaveSurgeryTicketInventoryValidator()
        {
            RuleFor(x => x.SaveSurgeyTicketInventoryRequest)
                .Must(x => x != null & x.Count > 0).WithMessage("One surgery is required");

            RuleForEach(x => x.SaveSurgeyTicketInventoryRequest)
                .SetValidator(new SaveSurgeryTicketInventoryRequestValidator());
        }
    }
}
