using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.UpdateSurgeryTicket
{
    public class UpdateSurgeryTicketPersonnelValidator : AbstractValidator<UpdateSurgeryTicketPersonnel>
    {
        public UpdateSurgeryTicketPersonnelValidator()
        {
            RuleFor(x => x.SurgeryRole)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Surgery role is required")
                .MaximumLength(500).WithMessage("Maximum length of 500 chars");

        }
    }
}
