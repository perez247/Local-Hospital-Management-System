using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.SaveSurgeryTicketInventory
{
    public class SaveSurgeryTicketInventoryRequestValidator : AbstractValidator<SaveSurgeyTicketInventoryRequest>
    {
        public SaveSurgeryTicketInventoryRequestValidator()
        {
            RuleFor(x => x.DoctorsPrescription)
                .MaximumLength(1000).WithMessage("Maximum of 1000 chars");
        }
    }
}
