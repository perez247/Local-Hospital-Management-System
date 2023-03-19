using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.UpdateTicket
{
    public class UpdateTicketValidator : AbstractValidator<UpdateTicketCommand>
    {
        public UpdateTicketValidator()
        {
            RuleFor(x => x.OverallDescription)
                .MaximumLength(5000).WithMessage("Not more than 500 chars");

            RuleFor(x => x.AppTicketStatus)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Payment status is required")
                .Must(x => CommonValidators.EnumsContains<AppTicketStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppTicketStatus)))}");

        }
    }
}
