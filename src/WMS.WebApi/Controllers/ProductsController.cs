using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Commands.Products;
using WMS.Application.DTOs;
using WMS.Application.Queries.Products;

namespace WMS.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        return result.IsSuccess && result.Value != null ? Ok(result.Value) : NotFound();
    }

    [HttpGet("sku/{sku}")]
    public async Task<ActionResult<ProductDto>> GetBySku(string sku)
    {
        var result = await _mediator.Send(new GetProductBySkuQuery(sku));
        return result.IsSuccess && result.Value != null ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateProductDto dto)
    {
        var result = await _mediator.Send(new CreateProductCommand(dto, "system"));
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : BadRequest(result.Error);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        var result = await _mediator.Send(new UpdateProductCommand(id, dto, "system"));
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}
