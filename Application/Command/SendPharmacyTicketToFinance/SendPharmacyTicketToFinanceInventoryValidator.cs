using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.SendPharmacyTicketToFinance
{
    public class SendPharmacyTicketToFinanceInventoryValidator : AbstractValidator<SendPharmacyTicketToFinanceInventory>
    {
        public SendPharmacyTicketToFinanceInventoryValidator()
        {
            RuleFor(x => x.AppTicketStatus)
                .Must(x => CommonValidators.EnumsContains<AppTicketStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppTicketStatus)))}");

            RuleFor(x => x.PrescribedQuantity)
                .Must(x => x.HasValue).WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Must be greater than 0");

            RuleFor(x => x.DepartmentDescription)
                .MaximumLength(1000).WithMessage("Maximum of 1000 chars")
                .When(x => !string.IsNullOrEmpty(x.DepartmentDescription));
        }
    }
}
