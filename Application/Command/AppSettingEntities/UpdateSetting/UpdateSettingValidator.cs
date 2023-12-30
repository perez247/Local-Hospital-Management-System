using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AppSettingEntities.UpdateSetting
{
    internal class UpdateSettingValidator : AbstractValidator<UpdateSettingCommand>
    {
        public UpdateSettingValidator()
        {
            RuleFor(x => x.AppSettingType)
                .Must(x => CommonValidators.EnumsContains<AppSettingType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppSettingType)))}")
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Payment status is required");

            RuleFor(x => x.Data)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Data is required for update");
        }
    }
}
