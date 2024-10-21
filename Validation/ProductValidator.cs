using FluentValidation;
using ProductManagementAPI.DTOs;

namespace ProductManagementAPI.Validation {
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("Product must have a valid CategoryId.");
        }
    }
}
