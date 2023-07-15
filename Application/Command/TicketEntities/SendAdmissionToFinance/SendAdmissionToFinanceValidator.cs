using Application.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.SendAdmissionToFinance
{
    public class SendAdmissionToFinanceValidator : AbstractValidator<SendAdmissionToFinanceCommand>
    {
        public SendAdmissionToFinanceValidator()
        {
            RuleFor(x => x.TicketInventories)
                .Must(x => x != null && x.Count() > 0).WithMessage("At least one ticket inventory is required");

            RuleForEach(x => x.TicketInventories)
                .SetValidator(new SendTicketToFinanceRequestValidator());

            RuleForEach(x => x.TicketInventories)
                .Must(x => x.AdmissionStartDate.Value > DateTime.Today)
                .Must(x => x.AdmissionStartDate.HasValue).WithMessage("Admission Start Date is required");
        }
    }
}
