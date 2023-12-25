using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.BulkSave
{
    public class BulkSaveItemValidator: AbstractValidator<BulkSaveItemRequest>
    {
        public BulkSaveItemValidator()
        {
            RuleFor(x => x.Name)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Maximum of 100 chars");
                //.Matches("^[-a-zA-Z0-9._ ]*$").WithMessage("Only letters, numbers, periods and underscore");

            RuleFor(x => x.Type)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Inventory type is required")
                .Must(x => CommonValidators.EnumsContains<AppInventoryType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppInventoryType)))}");
        }
    }
}
