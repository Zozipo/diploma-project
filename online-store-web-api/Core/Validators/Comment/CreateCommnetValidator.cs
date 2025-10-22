using Core.Models.Comment;
using FluentValidation;

namespace Core.Validators.Comment
{
    public class CreateCommnetValidator : AbstractValidator<CreateCommentDto>
    {
            public CreateCommnetValidator()
            {
                RuleFor(dto => dto.Text)
                    .NotEmpty().WithMessage("Text is required.")
                    .MinimumLength(3).WithMessage("Text must be at least 3 characters long.")
                    .MaximumLength(500).WithMessage("Text cannot exceed 500 characters.");

               
                RuleFor(dto => dto.ProductId)
                    .GreaterThanOrEqualTo(0).WithMessage("ProductId must be greater than or equal to 0.");

               
        }
        
    }
}
