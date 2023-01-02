using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.CreateMonthPayment
{
    public class CreateMonthPaymentValidator : AbstractValidator<CreateMonthPaymentCommand>
    {
        public CreateMonthPaymentValidator()
        {
            RuleFor(x => x.Date)
                .Must(x => x.HasValue).WithMessage("Date is required");
        }
    }
}
