using Application.Command.TicketEntities.AddPharmacyTicketInventory;
using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.SaveTicketAndInventory
{
    public class SaveTicketAndInventoryValidator : AbstractValidator<SaveTicketAndInventoryCommand>
    {
        public SaveTicketAndInventoryValidator() 
        {
            RuleFor(x => x.OverallDescription)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Overal Description is required")
                .MinimumLength(3).WithMessage("A minimum of 3 chars")
                .MaximumLength(1000).WithMessage("A maximum of 1000 chars");

            RuleFor(x => x.AppInventoryType)
                .Must(x => CommonValidators.EnumsContains<AppInventoryType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppInventoryType)))}");

            RuleFor(x => x.TicketInventories)
                .Must(x => x != null && x.Count > 0).WithMessage("Inventory is required");

            RuleForEach(x => x.TicketInventories)
                .SetValidator(x => new SaveTicketAndInventoryRequestValidator())
                .When(x => x != null && x.TicketInventories.Count > 0);
        }
    }
}
