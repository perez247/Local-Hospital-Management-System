using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.AddPharmacyTicketInventory
{
    public class AddPharmacyTicketRequestValidator : AbstractValidator<AddPharmacyTicketRequest>
    {
        public AddPharmacyTicketRequestValidator()
        {
            RuleFor(x => x.DoctorsPrescription)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.DoctorsPrescription));

            RuleFor(x => x.PrescribedQuantity)
                .MaximumLength(1000)
                .Must(x => Int32.TryParse(x, out int result)).WithMessage("Must be a number")
                .When(x => !string.IsNullOrEmpty(x.PrescribedQuantity));
        }
    }
}
