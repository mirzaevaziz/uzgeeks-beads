using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Commands.Products;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty()
            .WithMessage("SKU is required")
            .MaximumLength(50)
            .WithMessage("SKU must not exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative");

        RuleFor(x => x.Weight).GreaterThan(0).WithMessage("Weight must be positive");

        RuleFor(x => x.ReorderLevel)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Reorder level must be non-negative");

        RuleFor(x => x.ReorderQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Reorder quantity must be non-negative");
    }
}
