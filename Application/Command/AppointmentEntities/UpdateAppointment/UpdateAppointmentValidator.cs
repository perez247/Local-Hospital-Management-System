using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AppointmentEntities.UpdateAppointment
{
    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentValidator()
        {
            RuleFor(x => x.AppointmentDate)
                //.Must(x => x.HasValue).WithMessage("Appointment date is required")
                .GreaterThan(DateTime.Now).WithMessage("Appointment date must be greater than now")
                .When(x => x.AppointmentDate.HasValue);

            RuleFor(x => x.OverallDescription)
                //.Must(x => x.HasValue).WithMessage("Appointment date is required")
                .MaximumLength(5000).WithMessage("Characters must not be greater than 5000")
                .When(x => !string.IsNullOrEmpty(x.OverallDescription));
        }
    }
}
