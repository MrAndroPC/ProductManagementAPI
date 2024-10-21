using FluentValidation;
using ProductManagementAPI.DTOs;

namespace ProductManagementAPI.Validation
{
    public class ProductCategoryValidator : AbstractValidator<ProductCategoryDto>
    {
        public ProductCategoryValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Category name is required.");
        }
    }

}
