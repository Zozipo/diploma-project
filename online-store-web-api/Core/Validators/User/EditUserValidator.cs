using FluentValidation;
using Core.Models.User;

namespace Core.Validators.User
{
    public class EditUserValidator : AbstractValidator<UpdateUserDto>
    {
        public EditUserValidator()
        {
            RuleFor(dto => dto.UserName)
                .MinimumLength(3).WithMessage("UserName must be at least 3 characters long.")
                .MaximumLength(255).WithMessage("UserName cannot exceed 255 characters.")
                .When(dto => !string.IsNullOrEmpty(dto.UserName));

            RuleFor(dto => dto.FirstName)
                .MinimumLength(3).WithMessage("First name must be at least 3 characters long.")
                .MaximumLength(255).WithMessage("First name cannot exceed 255 characters.")
                .When(dto => !string.IsNullOrEmpty(dto.FirstName));

            RuleFor(dto => dto.LastName)
                .MinimumLength(3).WithMessage("Last name must be at least 3 characters long.")
                .MaximumLength(255).WithMessage("Last name cannot exceed 255 characters.")
                .When(dto => !string.IsNullOrEmpty(dto.LastName));

            RuleFor(dto => dto.Email)
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.")
                .EmailAddress().WithMessage("Invalid Email address.")
                .When(dto => !string.IsNullOrEmpty(dto.Email));

            RuleFor(dto => dto.PhoneNumber)
                .Matches(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$").WithMessage("Invalid PhoneNumber format.")
                .When(dto => !string.IsNullOrEmpty(dto.PhoneNumber));

            RuleFor(dto => dto.Position)
                .MaximumLength(255).WithMessage("Position cannot exceed 255 characters.")
                .When(dto => !string.IsNullOrEmpty(dto.Position));
        }
    }
}
