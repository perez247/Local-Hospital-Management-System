using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.BulkSave
{
    public class BulkSaveValidator: AbstractValidator<BulkSaveCommand>
    {
        public BulkSaveValidator()
        {
            RuleFor(x => x.Items)
                .Must(x => x != null && x.Count() > 0).WithMessage("At least one item is required")
                .Must(x => x != null && x.Count() <= 100).WithMessage("Not more than 100 items per upload");

            RuleForEach(x => x.Items)
                .SetValidator(new BulkSaveItemValidator());
        }
    }
}
