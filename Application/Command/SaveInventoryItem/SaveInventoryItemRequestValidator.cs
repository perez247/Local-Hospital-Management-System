using Application.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.SaveInventoryItem
{
    public class SaveInventoryItemRequestValidator : AbstractValidator<SaveInventoryItemRequest>
    {
        public SaveInventoryItemRequestValidator()
        {
            RuleFor(x => x.InventoryId)
                .Must(CommonValidators.BeValidGuid).WithMessage("Invalid id");
        }
    }
}
