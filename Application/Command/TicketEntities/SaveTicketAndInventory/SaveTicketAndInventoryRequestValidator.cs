using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.SaveTicketAndInventory
{
    public class SaveTicketAndInventoryRequestValidator : AbstractValidator<SaveTicketAndInventoryRequest>
    {
        public SaveTicketAndInventoryRequestValidator()
        {
            RuleFor(x => x.DoctorsPrescription)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.DoctorsPrescription));

            RuleFor(x => x.PrescribedQuantity)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.PrescribedQuantity));
        }
    }
}
