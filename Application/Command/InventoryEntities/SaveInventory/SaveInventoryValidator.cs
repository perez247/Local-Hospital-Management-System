using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.SaveInventory
{
    public class SaveInventoryValidator : AbstractValidator<SaveInventoryCommand>
    {
        public SaveInventoryValidator()
        {
            RuleFor(x => x.Name)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Name is required")
                .Matches("^[a-zA-Z0-9._ ]*$").WithMessage("Only letters, numbers, periods and underscore");

            RuleFor(x => x.AppInventoryType)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Inventory type is required")
                .Must(x => CommonValidators.EnumsContains<AppInventoryType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppInventoryType)))}");

            RuleFor(x => x.Profile)
                .Must(x => CommonValidators.IsBase64String(x)).WithMessage("Invalid Image")
                .When(x => !string.IsNullOrEmpty(x.Profile));
        }
    }
}
