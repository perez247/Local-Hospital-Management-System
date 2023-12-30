using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.FinancialRecordEntities.InitialPayment
{
    public class InitialPaymentTicketInventoryValidator : AbstractValidator<InitialPaymentTicketInventory>
    {
        public InitialPaymentTicketInventoryValidator()
        {
            RuleFor(x => x.AppTicketStatus)
                .Must(x => CommonValidators.EnumsContains<AppTicketStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppTicketStatus)))}");

            RuleFor(x => x.CurrentPrice)
                .Must(x => x.HasValue).WithMessage("Current Price is required")
                .GreaterThanOrEqualTo(0).WithMessage("Must be greater than 0");

            RuleFor(x => x.TotalPrice)
                .Must(x => x.HasValue).WithMessage("Current Price is required")
                .GreaterThanOrEqualTo(0).WithMessage("Must be greater than 0");

            RuleFor(x => x.PrescribedQuantity)
                .Must(x => x.HasValue).WithMessage("Current Price is required")
                .GreaterThanOrEqualTo(0).WithMessage("Must be greater than 0");
        }
    }
}
