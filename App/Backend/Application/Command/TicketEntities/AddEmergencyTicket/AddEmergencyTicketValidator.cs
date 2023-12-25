using Application.Command.TicketEntities.SaveTicketAndInventory;
using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.TicketEntities.AddEmergencyTicket
{
    public class AddEmergencyTicketValidator : AbstractValidator<AddEmergencyTicketCommand>
    {
        public AddEmergencyTicketValidator()
        {
            RuleFor(x => x.OverallTicketDescription)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Overal Ticket Description is required")
                .MinimumLength(3).WithMessage("A minimum of 3 chars")
                .MaximumLength(5000).WithMessage("A maximum of 5000 chars");

            RuleFor(x => x.OverallAppointmentDescription)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Overal Appointment Description is required")
                .MinimumLength(3).WithMessage("A minimum of 3 chars")
                .MaximumLength(5000).WithMessage("A maximum of 5000 chars");

            RuleFor(x => x.AppInventoryType)
                .Must(x => CommonValidators.EnumsContains<AppInventoryType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppInventoryType)))}");

            RuleFor(x => x.TicketInventories)
                .Must(x => x != null && x.Count > 0).WithMessage("At least one inventory is required");

            RuleForEach(x => x.TicketInventories)
                .SetValidator(new SaveTicketAndInventoryRequestValidator());
        }
    }
}
