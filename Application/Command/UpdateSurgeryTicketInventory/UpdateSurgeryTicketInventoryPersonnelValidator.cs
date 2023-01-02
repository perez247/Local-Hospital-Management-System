using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateSurgeryTicketInventory
{
    public class UpdateSurgeryTicketInventoryPersonnelValidator : AbstractValidator<UpdateSurgeryTicketInventoryPersonnel>
    {
        public UpdateSurgeryTicketInventoryPersonnelValidator()
        {
            RuleFor(entity => entity.SurgeryRole)
                .MaximumLength(250).WithMessage("Maximum of 250 char");

            RuleFor(entity => entity.Description)
                .MaximumLength(1000).WithMessage("Maximum of 1000 char");
        }
    }
}
