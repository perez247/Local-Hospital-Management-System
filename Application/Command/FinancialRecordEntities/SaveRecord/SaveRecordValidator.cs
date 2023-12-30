using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.FinancialRecordEntities.SaveRecord
{
    public class SaveRecordValidator: AbstractValidator<SaveRecordCommand>
    {
        public SaveRecordValidator()
        {
            RuleFor(x => x.Amount)
                .Must(x => x.HasValue).WithMessage("Amount is required");

            //RuleFor(x => x.Proof)
            //    .Must(x => CommonValidators.IsBase64String(x)).WithMessage("Invalid file uploaded");

            //RuleFor(x => x.PaymentType)
            //    .Must(x => CommonValidators.EnumsContains<PaymentType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(PaymentType)))}");

            RuleFor(x => x.Description)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Description is required")
                .MaximumLength(1000).WithMessage("Maximum of 1000 chars");

            RuleFor(x => x.AppCostType)
                .Must(x => CommonValidators.EnumsContains<AppCostType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppCostType)))}");
        
        }
    }
}
