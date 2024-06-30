using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.SaveTicketInventory
{
    public class SaveTicketInventoryDebtorValidator : AbstractValidator<SaveTicketInventoryDebtor>
    {
        public SaveTicketInventoryDebtorValidator()
        {
            RuleFor(x => x.Amount)
                .Must(x => x.HasValue).WithMessage("Amount is required")
                .Must(x => x > 0).WithMessage("Amount must be greater than 0");
        }
    }
}
