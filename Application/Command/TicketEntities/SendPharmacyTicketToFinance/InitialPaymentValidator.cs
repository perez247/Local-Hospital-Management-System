using Application.Command.FinancialRecordEntities.InitialPayment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.SendPharmacyTicketToFinance
{
    public class InitialPaymentValidator : AbstractValidator<InitialPaymentCommand>
    {
        public InitialPaymentValidator()
        {
            RuleFor(x => x.VatTotal)
                .Must(x => x.HasValue).WithMessage("Vat is required")
                .GreaterThan(0).WithMessage("Vat must be greater than 0 or equal");

            RuleFor(x => x.Total)
                .Must(x => x.HasValue).WithMessage("Vat is required")
                .GreaterThan(0).WithMessage("Vat must be greater than 0 or equal");

            RuleFor(x => x.SumTotal)
                .Must(x => x.HasValue).WithMessage("Vat is required")
                .GreaterThan(0).WithMessage("Vat must be greater than 0 or equal");

            RuleFor(x => x.TicketInventories)
                .Must(x => x != null && x.Count > 0).WithMessage("Ticket Inventories is required");

            RuleForEach(x => x.TicketInventories)
                .SetValidator(new InitialPaymentTicketInventoryValidator());

            RuleForEach(x => x.Payments)
                .SetValidator(new InitialPaymentPaymentValidator())
                .When(x => x.Payments != null && x.Payments.Count > 0);
        }
    }
}
