using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.AddInventoryDependencies
{
    public class AddInventoryDependenciesItemValidator : AbstractValidator<AddInventoryDependenciesItem>
    {
        public AddInventoryDependenciesItemValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(1).WithMessage("Must be greater than one");
        }
    }
}
