using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.ActivityLogEntities.GetActivityLog
{
    public class GetActivityLogQueryValidator : AbstractValidator<GetActivityLogQueryFilter>
    {
        public GetActivityLogQueryValidator()
        {
            RuleFor(x => x.ObjectType)
                .MaximumLength(255).WithMessage("Maximum length of 255")
                .When(x => !string.IsNullOrEmpty(x.ObjectType));

            RuleFor(x => x.ObjectId)
                .MaximumLength(255).WithMessage("Maximum length of 255")
                .When(x => !string.IsNullOrEmpty(x.ObjectType));

            RuleFor(x => x.ActionType)
                .MaximumLength(255).WithMessage("Maximum length of 255")
                .When(x => !string.IsNullOrEmpty(x.ObjectType));
        }
    }
}
