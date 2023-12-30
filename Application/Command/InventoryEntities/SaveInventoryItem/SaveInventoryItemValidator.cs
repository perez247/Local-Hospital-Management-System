using Application.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.SaveInventoryItem
{
    public class SaveInventoryItemValidator : AbstractValidator<SaveInventoryItemCommand>
    {
        public SaveInventoryItemValidator()
        {
            RuleForEach(x => x.InventoryItemRequests)
                .SetValidator(new SaveInventoryItemRequestValidator());
        }
    }
}
