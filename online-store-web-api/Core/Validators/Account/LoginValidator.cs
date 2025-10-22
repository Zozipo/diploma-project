using Core.Models.Account;
using FluentValidation;

namespace Core.Validators.Account
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid Email address.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(255).WithMessage("Password cannot exceed 255 characters.");
        }
    }
}