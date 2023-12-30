using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.FinancialRecordEntities.GetRevenue
{
    public class GetRevenueValidator: AbstractValidator<GetRevenueQuery>
    {
        public GetRevenueValidator()
        {
            RuleFor(x => x.StartDate)
                .Must(x => x.HasValue).WithMessage("Start Date is required")
                .Must((x, y, z) => x.StartDate < x.EndDate).WithMessage("Start Date must be less than End Date");

            RuleFor(x => x.EndDate)
                .Must(x => x.HasValue).WithMessage("End Date is required");

        }
    }
}
