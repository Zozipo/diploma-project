using Core.Models.Product;
using FluentValidation;

namespace Core.Validators.Product
{
    public class EditProductValidator : AbstractValidator<UpdateProductDto>
    {
        public EditProductValidator()
        {
            RuleFor(dto => dto.Title)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(4000).WithMessage("Description cannot exceed 4000 characters.");

            RuleFor(dto => dto.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(dto => dto.Rating)
                .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10.");

            RuleFor(dto => dto.CategoryId)
                .GreaterThanOrEqualTo(0).WithMessage("CategoryId must be greater than or equal to 0.");
        }
    }
}
