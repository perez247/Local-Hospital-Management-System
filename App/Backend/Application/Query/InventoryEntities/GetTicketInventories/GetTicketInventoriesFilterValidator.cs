using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetTicketInventories
{
    public class GetTicketInventoriesFilterValidator: AbstractValidator<GetTicketInventoriesFilter>
    {
        public GetTicketInventoriesFilterValidator()
        {
            RuleForEach(x => x.roles)
                .Must(x => CommonValidators.EnumsContains<AppInventoryType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppInventoryType)))}")
                .When(x => x.roles != null && x.roles.Count > 0);
        }
    }
}
