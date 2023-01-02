using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AddStaffTimeTable
{
    public class AddStaffTimeTableValidator : AbstractValidator<AddStaffTimeTableCommand>
    {
        public AddStaffTimeTableValidator()
        {
            RuleFor(x => x.StaffIds)
                .Must(x => x != null && x.Count > 0).WithMessage("At least one Id is required");

            RuleFor(x => x.ClockIn)
                .Must(x => x.HasValue).WithMessage("Clock in is required")
                .GreaterThan(DateTime.Now).WithMessage("Clock in must be in the future");

            RuleFor(x => x.ClockOut)
                .Must(x => x.HasValue).WithMessage("Clock out is required")
                .Must((x, y, z) => x.ClockIn < x.ClockOut).WithMessage("Clock out must be greater than clock in")
                .When(x => x.ClockIn.HasValue);
        }
    }
}
