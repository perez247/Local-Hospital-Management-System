using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetAppointmentCountInAMonth
{
    public class GetAppointmentCountInAMonthValidator : AbstractValidator<GetAppointmentCountInAMonthQuery>
    {
        public GetAppointmentCountInAMonthValidator()
        {
            RuleFor(x => x.Date)
                .Must(x => x.HasValue).WithMessage("Date is required");
        }
    }
}
