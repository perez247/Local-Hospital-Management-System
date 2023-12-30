using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.UpdateQuantity
{
    public class UpdateQuantityValidator: AbstractValidator<UpdateQuantityCommand>
    {
        public UpdateQuantityValidator()
        {
            RuleFor(x => x.Amount)
                .Must(x => x.Value > 0).WithMessage("Quantity must be greater than zero")
                .Must(x => x.HasValue).WithMessage("Quantity is required");

            RuleFor(x => x.Add)
                .Must(x => x.HasValue).WithMessage("Either Adding or substracting");

            RuleFor(x => x.Reason)
                .MaximumLength(10000).WithMessage("Not more than 10000 characters");
        }
    }
}
