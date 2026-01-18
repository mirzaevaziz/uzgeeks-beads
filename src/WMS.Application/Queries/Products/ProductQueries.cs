using WMS.Application.Common;
using WMS.Application.DTOs;

namespace WMS.Application.Queries.Products;

public record GetProductByIdQuery(Guid ProductId) : IQuery<Result<ProductDto?>>;

public record GetProductBySkuQuery(string Sku) : IQuery<Result<ProductDto?>>;

public record GetAllProductsQuery() : IQuery<Result<IEnumerable<ProductDto>>>;

public record GetProductsByCategoryQuery(string Category) : IQuery<Result<IEnumerable<ProductDto>>>;
