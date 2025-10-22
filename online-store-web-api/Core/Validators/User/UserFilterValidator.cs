using Core.Models;
using FluentValidation;

namespace Core.Validators.User
{
    public class FilterValidator : AbstractValidator<ItemsFilter>
    {
        public FilterValidator()
        {
            RuleFor(dto => dto.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal to 1.");

            RuleFor(dto => dto.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

            RuleFor(dto => dto.SearchTerm)
                .MaximumLength(255).WithMessage("SearchTerm cannot exceed 255 characters.");

            RuleFor(dto => dto.SortBy)
                .MaximumLength(255).WithMessage("SortBy cannot exceed 255 characters.");

            RuleFor(dto => dto.SortDirection)
                .Must(direction => direction == null || direction.ToLower() == "asc" || direction.ToLower() == "desc")
                .WithMessage("SortDirection must be 'asc', 'desc', or null.");
        }
    }
}