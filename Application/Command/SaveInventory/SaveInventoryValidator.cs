using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.SaveInventory
{
    public class SaveInventoryValidator : AbstractValidator<SaveInventoryCommand>
    {
        public SaveInventoryValidator()
        {
            RuleFor(x => x.Name)
                .Must(x => string.IsNullOrEmpty(x)).WithMessage("Name is required");
        }
    }
}
