﻿using Application.Annotations;
using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests
{
    public class SendTicketToFinanceRequestValidator : AbstractValidator<SendTicketToFinanceRequest>
    {
        public SendTicketToFinanceRequestValidator()
        {
            RuleFor(x => x.AppTicketStatus)
                .Must(x => CommonValidators.EnumsContains<AppTicketStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppTicketStatus)))}");

            RuleFor(x => x.PrescribedQuantity)
                .Must(x => x.HasValue).WithMessage("Quantity is required")
                .GreaterThanOrEqualTo(0).WithMessage("Must be greater than or equals 0");

            RuleFor(x => x.DepartmentDescription)
                .MaximumLength(1000).WithMessage("Maximum of 1000 chars")
                .When(x => !string.IsNullOrEmpty(x.DepartmentDescription));
        }
    }
}
