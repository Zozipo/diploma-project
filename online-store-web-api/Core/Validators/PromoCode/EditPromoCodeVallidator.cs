using Core.Models.PromoCode;
using FluentValidation;

namespace Core.Validators.PromoCode
{
    public class EditPromoCodeValidator : AbstractValidator<UpdatePromoCodeDto>
    {
        public EditPromoCodeValidator()
        {
            RuleFor(dto => dto.Code)
                .NotEmpty().WithMessage("Code is required.")
                .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.");

            RuleFor(dto => dto.DiscountPercentage)
                .InclusiveBetween(0, 100).WithMessage("Discount percentage must be between 0 and 100.");

            RuleFor(dto => dto.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(dto => dto.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(dto => dto.StartDate).WithMessage("End date must be greater than or equal to start date.");

            RuleFor(dto => dto.RemainingUses)
                .GreaterThanOrEqualTo(0).WithMessage("Remaining uses must be greater than or equal to 0.");
        }
    }
}
