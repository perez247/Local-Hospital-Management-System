using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateSurgerySummary
{
    public class UpdateSurgerySummaryValidator : AbstractValidator<UpdateSurgerySummaryCommand>
    {
        public UpdateSurgerySummaryValidator()
        {
            RuleFor(x => x.Summary)
                .Must(x => string.IsNullOrEmpty(x)).WithMessage("Summary is required")
                .MaximumLength(1000).WithMessage("Maximum of 1000 chars");
        }
    }
}
