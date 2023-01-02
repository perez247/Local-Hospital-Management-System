using Application.Command.AddLabTicketInventory;
using Application.Command.AddPharmacyTicketInventory;
using Application.Command.SaveSurgeryTicketInventory;
using Application.Command.UpdateLabTicketInventory;
using Application.Command.UpdatePharmacyTicketInventory;
using Application.Command.UpdateSurgeryTicketInventory;
using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.CreateEmergencyAppoitment
{
    public class CreateEmergencyAppointmentValidator : AbstractValidator<CreateEmergencyAppointmentCommand>
    {
        public CreateEmergencyAppointmentValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .Must(x => x.HasValue).WithMessage("Appointment date is required")
                .GreaterThan(DateTime.Now).WithMessage("Appointment date must be greater than now");

            RuleFor(x => x.AppInventoryType)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Inventory type is required")
                .Must(x => CommonValidators.EnumsContains<AppInventoryType>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppInventoryType)))}");

            RuleForEach(x => x.AddLabTicketInventoryRequest)
                .SetValidator(new AddLabTicketInventoryRequestValidator())
                .When(x => x.AddLabTicketInventoryRequest != null && x.AddLabTicketInventoryRequest.Count > 0);

            RuleForEach(x => x.UpdateLabTicketInventoryRequest)
                .SetValidator(new UpdateLabTicketInventoryRequestValidator())
                .When(x => x.UpdateLabTicketInventoryRequest != null && x.UpdateLabTicketInventoryRequest.Count > 0);

            RuleForEach(x => x.SaveSurgeyTicketInventoryRequest)
                .SetValidator(new SaveSurgeryTicketInventoryRequestValidator())
                .When(x => x.SaveSurgeyTicketInventoryRequest != null && x.SaveSurgeyTicketInventoryRequest.Count > 0);

            RuleForEach(x => x.UpdateSurgeryTicketInventoryRequest)
                .SetValidator(new UpdateSurgeryTicketInventoryRequestValidator())
                .When(x => x.UpdateSurgeryTicketInventoryRequest != null && x.UpdateSurgeryTicketInventoryRequest.Count > 0);

            RuleForEach(x => x.TicketInventories)
                .SetValidator(new AddPharmacyTicketRequestValidator())
                .When(x => x.TicketInventories != null && x.TicketInventories.Count > 0);

            RuleForEach(x => x.UpdatePharmacyTicketInventoryRequest)
                .SetValidator(new UpdatePharmacyTicketInventoryRequestValidator())
                .When(x => x.UpdatePharmacyTicketInventoryRequest != null && x.UpdatePharmacyTicketInventoryRequest.Count > 0);
        }
    }
}
