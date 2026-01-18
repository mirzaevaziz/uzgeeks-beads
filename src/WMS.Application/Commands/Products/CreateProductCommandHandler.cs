using WMS.Application.Common;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Domain.ValueObjects;

namespace WMS.Application.Commands.Products;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var dto = request.Product;

            // Check if product with same SKU exists
            if (await _productRepository.ExistsAsync(dto.Sku, cancellationToken))
            {
                return Result.Failure<Guid>($"Product with SKU '{dto.Sku}' already exists");
            }

            // Create value objects
            var price = Money.Create(dto.Price, dto.Currency);
            var weight = Quantity.Create(dto.Weight, dto.WeightUnit);
            var dimensions = Quantity.Create(dto.Dimensions, dto.DimensionsUnit);

            // Create product entity
            var product = Product.Create(
                dto.Sku,
                dto.Name,
                dto.Description,
                price,
                weight,
                dimensions,
                dto.ReorderLevel,
                dto.ReorderQuantity,
                dto.Category,
                request.CreatedBy
            );

            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(product.Id);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(ex.Message);
        }
    }
}
