using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetInventories
{
    public class GetInventoriesFilterValidator : AbstractValidator<GetInventoriesFilter>
    {
        public GetInventoriesFilterValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage("Must not be greater than 200")
                .Matches("^[a-zA-Z0-9._ ]*$").WithMessage("Only letters, numbers, periods and underscore")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.AppInventoryType)
                .Must(x => CommonValidators.EnumsContains<AppInventoryType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppInventoryType)))}")
                .When(x => !string.IsNullOrEmpty(x.AppInventoryType));

            RuleFor(x => x.Quantity)
                .Must(CommonValidators.BeInteger).WithMessage("Value is not a whole number")
                .When(x => !string.IsNullOrEmpty(x.Quantity));
        }
    }
}
