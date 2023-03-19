using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.FinancialRecordEntities.GetPendingUserContracts
{
    public class GetPendingUserContractsValidator : AbstractValidator<GetPendingUserContractsQuery>
    {
        public GetPendingUserContractsValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new GetPendingUserContractsFilterValidator())
                .When(x => x.Filter != null);
        }
    }
}
