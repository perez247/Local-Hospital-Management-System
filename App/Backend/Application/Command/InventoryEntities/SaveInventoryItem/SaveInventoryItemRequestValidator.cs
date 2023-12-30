using Application.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.SaveInventoryItem
{
    public class SaveInventoryItemRequestValidator : AbstractValidator<SaveInventoryItemRequest>
    {
        public SaveInventoryItemRequestValidator()
        {
            //RuleFor(x => x.Amount)
            //    .Must(CommonValidators.BeValidGuid).WithMessage("Invalid id");
        }
    }
}
