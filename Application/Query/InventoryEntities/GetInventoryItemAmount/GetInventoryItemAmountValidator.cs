using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetInventoryItemAmount
{
    public class GetInventoryItemAmountValidator : AbstractValidator<GetInventoryItemAmountQuery>
    {
        public GetInventoryItemAmountValidator()
        {
            RuleFor(x => x.AppInventories)
                .Must(x => x != null && x.Count() > 0).WithMessage("Inventories is required");
        }
    }
}
