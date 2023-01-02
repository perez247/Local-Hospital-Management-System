using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.ConcludePharmacyTicket
{
    public class ConcludePharmacyTicketValidator : AbstractValidator<ConcludePharmacyTicketCommand>
    {
        public ConcludePharmacyTicketValidator()
        {
            RuleFor(x => x.ConcludePharmacyTicketRequest)
                .Must(x => x != null && x.Count() > 0).WithMessage("At least one ticket inventory is required");

            RuleForEach(x => x.ConcludePharmacyTicketRequest)
                .SetValidator(new ConcludePharmacyTicketRequestValidator());
        }
    }
}
