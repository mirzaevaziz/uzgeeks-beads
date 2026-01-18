using System.Collections.ObjectModel;

namespace WMS.Application.DTOs;

public record OrderDto
{
    public Guid Id { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public Guid WarehouseId { get; init; }
    public DateTime OrderDate { get; init; }
    public DateTime? ExpectedDate { get; init; }
    public DateTime? CompletedDate { get; init; }
    public string CustomerReference { get; init; } = string.Empty;
    public decimal TotalValue { get; init; }
    public string Currency { get; init; } = string.Empty;
    public Collection<OrderLineDto> OrderLines { get; init; } = new();
}

public record OrderLineDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal LineTotal { get; init; }
    public int QuantityPicked { get; init; }
    public int QuantityShipped { get; init; }
}

public record CreateOrderDto
{
    public string OrderNumber { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public Guid WarehouseId { get; init; }
    public DateTime ExpectedDate { get; init; }
    public string CustomerReference { get; init; } = string.Empty;
    public AddressDto? ShippingAddress { get; init; }
    public Collection<CreateOrderLineDto> OrderLines { get; init; } = new();
}

public record CreateOrderLineDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public string Currency { get; init; } = "USD";
}

public record AddressDto
{
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
}
