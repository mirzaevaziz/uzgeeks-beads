using WMS.Domain.Common;

namespace WMS.Domain.Events;

public record StockIncreasedEvent(Guid InventoryId, Guid ProductId, int Quantity) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid EventId { get; init; } = Guid.NewGuid();
}

public record StockDecreasedEvent(Guid InventoryId, Guid ProductId, int Quantity) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid EventId { get; init; } = Guid.NewGuid();
}

public record StockReservedEvent(Guid InventoryId, Guid ProductId, int Quantity) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid EventId { get; init; } = Guid.NewGuid();
}

public record StockReservationReleasedEvent(Guid InventoryId, Guid ProductId, int Quantity)
    : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid EventId { get; init; } = Guid.NewGuid();
}

public record StockAdjustedEvent(
    Guid InventoryId,
    Guid ProductId,
    int OldQuantity,
    int NewQuantity,
    string Reason
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid EventId { get; init; } = Guid.NewGuid();
}
