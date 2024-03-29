﻿using Application.Validations;
using FluentValidation;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests
{
    public class ConcludeTicketRequestValidator : AbstractValidator<ConcludeTicketRequest>
    {
        public ConcludeTicketRequestValidator()
        {
            RuleFor(x => x.AppTicketStatus)
                .Must(x => CommonValidators.EnumsContains<AppTicketStatus>(x)).WithMessage($"Only: {string.Join(", ", Enum.GetNames(typeof(AppTicketStatus)))}");

            RuleForEach(x => x.Proof)
                .Must(CommonValidators.IsBase64String).WithMessage("Invalid image")
                .When(x => x.Proof != null && x.Proof.Count > 0);
        }
    }
}
