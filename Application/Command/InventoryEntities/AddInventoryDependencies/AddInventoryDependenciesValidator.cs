using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.AddInventoryDependencies
{
    public class AddInventoryDependenciesValidator : AbstractValidator<AddInventoryDependenciesCommand>
    {
        public AddInventoryDependenciesValidator()
        {
            RuleFor(x => x.Dependencies)
               .Must(x => x != null).WithMessage("Dependencies should not be null");

            RuleFor(x => x.Dependencies)
               .Must(x => x.Count <= 10 && x.Count >= 0).WithMessage("Maximum of 10 dependencies");
        }
    }
}
