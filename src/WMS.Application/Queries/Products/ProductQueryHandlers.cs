using AutoMapper;
using WMS.Application.Common;
using WMS.Application.DTOs;
using WMS.Domain.Interfaces;

namespace WMS.Application.Queries.Products;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Result<ProductDto?>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto?>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(
                request.ProductId,
                cancellationToken
            );
            var productDto = _mapper.Map<ProductDto?>(product);
            return Result.Success(productDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<ProductDto?>(ex.Message);
        }
    }
}

public class GetProductBySkuQueryHandler : IQueryHandler<GetProductBySkuQuery, Result<ProductDto?>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductBySkuQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto?>> Handle(
        GetProductBySkuQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var product = await _productRepository.GetBySkuAsync(request.Sku, cancellationToken);
            var productDto = _mapper.Map<ProductDto?>(product);
            return Result.Success(productDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<ProductDto?>(ex.Message);
        }
    }
}

public class GetAllProductsQueryHandler
    : IQueryHandler<GetAllProductsQuery, Result<IEnumerable<ProductDto>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ProductDto>>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Result.Success(productDtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<ProductDto>>(ex.Message);
        }
    }
}
