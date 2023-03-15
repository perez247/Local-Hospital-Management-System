using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetTickets
{
    public class GetTicketFilterValidator : AbstractValidator<GetTicketsQueryFilter>
    {
        public GetTicketFilterValidator()
        {
            RuleFor(x => x.AppInventoryType)
                .Must(x => CommonValidators.EnumsContains<AppInventoryType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppInventoryType)))}")
                .When(x => !string.IsNullOrEmpty(x.AppInventoryType));

            RuleFor(x => x.AppTicketStatus)
                .Must(x => CommonValidators.EnumsContains<AppTicketStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppTicketStatus)))}")
                .When(x => !string.IsNullOrEmpty(x.AppInventoryType));
        }
    }
}
