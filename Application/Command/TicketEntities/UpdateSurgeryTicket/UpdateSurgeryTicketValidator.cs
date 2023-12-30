using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.UpdateSurgeryTicket
{
    public class UpdateSurgeryTicketValidator : AbstractValidator<UpdateSurgeryTicketCommand>
    {
        public UpdateSurgeryTicketValidator()
        {
            RuleFor(x => x.SurgeryDate)
                .Must(x => x > DateTime.UtcNow).WithMessage("Surgery cannot be now")
                .When(x => x.SurgeryDate.HasValue);

            RuleFor(x => x.SurgeryTicketPersonnels)
                .Must(x => x != null && x.Count > 0).WithMessage("At least one staff is required");

            RuleForEach(x => x.SurgeryTicketPersonnels)
                .SetValidator(new UpdateSurgeryTicketPersonnelValidator());
        }
    }
}
