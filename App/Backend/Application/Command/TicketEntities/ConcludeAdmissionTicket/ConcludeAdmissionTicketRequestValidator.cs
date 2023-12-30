using Application.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.ConcludeAdmissionTicket
{
    public class ConcludeAdmissionTicketRequestValidator : AbstractValidator<ConcludeAdmissionTicketRequest>
    {
        public ConcludeAdmissionTicketRequestValidator()
        {
            Include(new ConcludeTicketRequestValidator());

            RuleFor(x => x.TotalPrice)
                .Must(x => x.HasValue).WithMessage("Concluded price is required");

            RuleFor(x => x.TotalPrice)
                .Must(x => x.HasValue && x.Value > 0).WithMessage("Concluded price must be greater than zero");

            RuleFor(x => x.AdmissionEndDate)
                .Must(x => x.HasValue).WithMessage("Admission end date is required");
        }
    }
}
