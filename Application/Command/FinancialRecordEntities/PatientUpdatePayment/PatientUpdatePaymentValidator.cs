using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.FinancialRecordEntities.PatientUpdatePayment
{
    public class PatientUpdatePaymentValidator : AbstractValidator<PatientUpdatePaymentCommand>
    {
        public PatientUpdatePaymentValidator()
        {
            RuleFor(x => x.Payments)
                .Must(x => x != null && x.Count > 0).WithMessage("Payments is required");

            RuleForEach(x => x.Payments)
                .SetValidator(new PatientUpdatePaymentPaymentValidator());
        }
    }
}
