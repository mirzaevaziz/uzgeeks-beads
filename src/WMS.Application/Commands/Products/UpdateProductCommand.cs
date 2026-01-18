using WMS.Application.Common;
using WMS.Application.DTOs;

namespace WMS.Application.Commands.Products;

public record UpdateProductCommand(Guid ProductId, UpdateProductDto Product, string UpdatedBy)
    : ICommand<Result>;
