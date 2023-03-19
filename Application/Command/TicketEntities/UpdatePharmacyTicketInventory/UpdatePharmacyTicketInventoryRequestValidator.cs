using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.UpdatePharmacyTicketInventory
{
    public class UpdatePharmacyTicketInventoryRequestValidator : AbstractValidator<UpdatePharmacyTicketInventoryRequest>
    {
        public UpdatePharmacyTicketInventoryRequestValidator()
        {
            RuleFor(x => x.AppTicketStatus)
                .Must(x => CommonValidators.EnumsContains<AppTicketStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppTicketStatus)))}");

            RuleFor(x => x.StaffObservation)
                .MaximumLength(1000).WithMessage("Maximum of 1000 chars");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Maximum of 1000 chars");

            RuleForEach(x => x.Proof)
                .Must(x => CommonValidators.IsBase64String(x)).WithMessage("Invalid image")
                .When(x => x.Proof != null && x.Proof.Count > 0);
        }
    }
}
