using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdatePaymentForMonth
{
    public class UpdatePaymentForMonthValidator : AbstractValidator<UpdatePaymentForMonthCommand>
    {
        public UpdatePaymentForMonthValidator()
        {
            RuleFor(x => x.Date)
                .Must(x => x.HasValue).WithMessage("Date is required");

            RuleFor(x => x.StaffIds)
                .Must(x => x != null && x.Count > 0).WithMessage("At least one Id is required");
        }
    }
}
