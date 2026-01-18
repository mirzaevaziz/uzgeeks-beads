using WMS.Application.Common;
using WMS.Application.DTOs;

namespace WMS.Application.Commands.Products;

public record CreateProductCommand(CreateProductDto Product, string CreatedBy)
    : ICommand<Result<Guid>>;
