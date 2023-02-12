using FluentValidation;

namespace Application.Command.CreateStaff
{
    public class CreateStaffValidation : AbstractValidator<CreateStaffCommand>
    {
        public CreateStaffValidation()
        {

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._ ]*$").WithMessage("Only letters, numbers, periods and underscore");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._ ]*$").WithMessage("Only letters, numbers, periods and underscore");

            RuleFor(x => x.OtherName)
                .NotEmpty().WithMessage("Other Name is required")
                .MaximumLength(200).WithMessage("Maximum of 200 chars")
                .Matches("^[a-zA-Z0-9._ ]*$").WithMessage("Only letters, numbers, periods and underscore")
                .When(x => !string.IsNullOrEmpty(x.OtherName));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is invalid");
                //.MustAsync(async (x, y, z) => await _auth.IsEmailAvailable(x.Email)).WithMessage("E-mail belongs to another user");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(2000).WithMessage("Maximum of 2000 chars");

            //RuleFor(x => x.Password)
            //    .Matches("^(?=.*[a-z])(?=.*[A-Z]).+$").WithMessage("One lowercase and uppercase")
            //    .MinimumLength(6).WithMessage("Minimum of 5 charcters")
            //    .MaximumLength(20).WithMessage("Maximum of 20 charcters")
            //    .NotEmpty().WithMessage("Password is required");
        }
    }
}
