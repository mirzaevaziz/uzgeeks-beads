using WMS.Application.Common;
using WMS.Domain.Interfaces;
using WMS.Domain.ValueObjects;

namespace WMS.Application.Commands.Products;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(
                request.ProductId,
                cancellationToken
            );
            if (product == null)
            {
                return Result.Failure($"Product with ID '{request.ProductId}' not found");
            }

            var price = Money.Create(request.Product.Price, request.Product.Currency);

            product.UpdateDetails(
                request.Product.Name,
                request.Product.Description,
                price,
                request.UpdatedBy
            );

            await _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
