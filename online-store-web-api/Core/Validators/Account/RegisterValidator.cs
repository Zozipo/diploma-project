using Core.Models.Account;
using FluentValidation;

namespace Core.Validators.Account
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(dto => dto.FirstName)
                .NotEmpty().WithMessage("FirstName is required.")
                .MinimumLength(1).WithMessage("FirstName must be at least 1 characters long.")
                .MaximumLength(255).WithMessage("FirstName cannot exceed 255 characters.");

            RuleFor(dto => dto.LastName)
                .NotEmpty().WithMessage("LastName is required.")
                .MinimumLength(1).WithMessage("LastName must be at least 1 characters long.")
                .MaximumLength(255).WithMessage("LastName cannot exceed 255 characters.");

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid Email address.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

            RuleFor(dto => dto.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .Matches(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$").WithMessage("Invalid PhoneNumber format.")
                .MaximumLength(255).WithMessage("PhoneNumber cannot exceed 255 characters.");

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(255).WithMessage("Password cannot exceed 255 characters.");

            //RuleFor(dto => dto.PasswordConfirmation)
              //  .Equal(dto => dto.Password).WithMessage("PasswordConfirmation must match the Password.");
        }
    }
}