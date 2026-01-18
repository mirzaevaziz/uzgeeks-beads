using WMS.Domain.Common;
using WMS.Domain.Events;

namespace WMS.Domain.Entities;

public class Inventory : BaseEntity
{
    public Guid ProductId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public Guid LocationId { get; private set; }
    public int QuantityOnHand { get; private set; }
    public int QuantityReserved { get; private set; }
    public int QuantityAvailable => QuantityOnHand - QuantityReserved;
    public DateTime LastStockCheck { get; private set; }

    private Inventory() { }

    public static Inventory Create(
        Guid productId,
        Guid warehouseId,
        Guid locationId,
        int initialQuantity,
        string createdBy
    )
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty", nameof(productId));
        if (warehouseId == Guid.Empty)
            throw new ArgumentException("Warehouse ID cannot be empty", nameof(warehouseId));
        if (locationId == Guid.Empty)
            throw new ArgumentException("Location ID cannot be empty", nameof(locationId));
        if (initialQuantity < 0)
            throw new ArgumentException(
                "Initial quantity cannot be negative",
                nameof(initialQuantity)
            );

        var inventory = new Inventory
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            LocationId = locationId,
            QuantityOnHand = initialQuantity,
            QuantityReserved = 0,
            LastStockCheck = DateTime.UtcNow,
            CreatedBy = createdBy,
        };

        return inventory;
    }

    public void IncreaseStock(int quantity, string updatedBy)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        QuantityOnHand += quantity;
        MarkAsUpdated(updatedBy);

        AddDomainEvent(new StockIncreasedEvent(Id, ProductId, quantity));
    }

    public void DecreaseStock(int quantity, string updatedBy)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (QuantityAvailable < quantity)
            throw new InvalidOperationException("Insufficient available stock");

        QuantityOnHand -= quantity;
        MarkAsUpdated(updatedBy);

        AddDomainEvent(new StockDecreasedEvent(Id, ProductId, quantity));
    }

    public void ReserveStock(int quantity, string updatedBy)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (QuantityAvailable < quantity)
            throw new InvalidOperationException("Insufficient available stock to reserve");

        QuantityReserved += quantity;
        MarkAsUpdated(updatedBy);

        AddDomainEvent(new StockReservedEvent(Id, ProductId, quantity));
    }

    public void ReleaseReservation(int quantity, string updatedBy)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (QuantityReserved < quantity)
            throw new InvalidOperationException("Cannot release more than reserved");

        QuantityReserved -= quantity;
        MarkAsUpdated(updatedBy);

        AddDomainEvent(new StockReservationReleasedEvent(Id, ProductId, quantity));
    }

    public void AdjustStock(int newQuantity, string reason, string updatedBy)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(newQuantity));

        var oldQuantity = QuantityOnHand;
        QuantityOnHand = newQuantity;
        LastStockCheck = DateTime.UtcNow;
        MarkAsUpdated(updatedBy);

        AddDomainEvent(new StockAdjustedEvent(Id, ProductId, oldQuantity, newQuantity, reason));
    }
}
