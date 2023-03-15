using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddAppointment
{
    public class AddAppointmentValidator : AbstractValidator<AppAppointmentCommand>
    {
        public AddAppointmentValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .Must(x => x.HasValue).WithMessage("Appointment date is required")
                .GreaterThan(DateTime.Today).WithMessage("Appointment date must not be in the past");
        }
    }
}
