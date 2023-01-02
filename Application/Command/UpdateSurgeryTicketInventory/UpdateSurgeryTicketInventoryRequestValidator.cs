using Application.Command.SaveSurgeryTicketInventory;
using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateSurgeryTicketInventory
{
    public class UpdateSurgeryTicketInventoryRequestValidator : AbstractValidator<UpdateSurgeryTicketInventoryRequest>
    {
        public UpdateSurgeryTicketInventoryRequestValidator()
        {

            //RuleFor(x => x.SurgeryTicketStatus)
            //    .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Surgery status is required")
            //    .Must(x => CommonValidators.EnumsContains<SurgeryTicketStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(SurgeryTicketStatus)))}");

            RuleForEach(x => x.UpdateSurgeryTicketInventoryPersonnel)
                .SetValidator(new UpdateSurgeryTicketInventoryPersonnelValidator())
                .When(x => x.UpdateSurgeryTicketInventoryPersonnel != null && x.UpdateSurgeryTicketInventoryPersonnel.Count > 0);

        }
    }
}
