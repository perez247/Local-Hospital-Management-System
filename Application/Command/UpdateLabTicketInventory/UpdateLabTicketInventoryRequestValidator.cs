using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateLabTicketInventory
{
    public class UpdateLabTicketInventoryRequestValidator : AbstractValidator<UpdateLabTicketInventoryRequest>
    {
        public UpdateLabTicketInventoryRequestValidator()
        {
            RuleFor(x => x.DateOfLabTest)
                .Must(x => x.HasValue).WithMessage("Date of test is request");
        }
    }
}
