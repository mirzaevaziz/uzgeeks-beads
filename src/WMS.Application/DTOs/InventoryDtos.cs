namespace WMS.Application.DTOs;

public record InventoryDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public Guid WarehouseId { get; init; }
    public string WarehouseName { get; init; } = string.Empty;
    public Guid LocationId { get; init; }
    public string LocationCode { get; init; } = string.Empty;
    public int QuantityOnHand { get; init; }
    public int QuantityReserved { get; init; }
    public int QuantityAvailable { get; init; }
    public DateTime LastStockCheck { get; init; }
}

public record AdjustInventoryDto
{
    public int NewQuantity { get; init; }
    public string Reason { get; init; } = string.Empty;
}

public record ReserveStockDto
{
    public int Quantity { get; init; }
}
