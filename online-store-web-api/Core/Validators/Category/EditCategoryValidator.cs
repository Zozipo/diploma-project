using Core.Models.Category;
using FluentValidation;

namespace Core.Validators.Category
{
    public class EditCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public EditCategoryValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(4000).WithMessage("Description cannot exceed 4000 characters.");

            RuleFor(dto => dto.ParentCategoryId)
                .GreaterThanOrEqualTo(0).WithMessage("ParentCategoryId must be greater than or equal to 0.");
        }
    }
}
