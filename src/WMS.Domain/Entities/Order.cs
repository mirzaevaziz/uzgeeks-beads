using WMS.Domain.Common;
using WMS.Domain.ValueObjects;

namespace WMS.Domain.Entities;

public enum OrderStatus
{
    Pending,
    Confirmed,
    InProgress,
    Packed,
    Shipped,
    Delivered,
    Cancelled,
}

public enum OrderType
{
    Inbound, // Receiving
    Outbound, // Shipping
}

public class Order : BaseEntity
{
    public string OrderNumber { get; private set; } = string.Empty;
    public OrderType Type { get; private set; }
    public OrderStatus Status { get; private set; }
    public Guid WarehouseId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? ExpectedDate { get; private set; }
    public DateTime? CompletedDate { get; private set; }
    public string CustomerReference { get; private set; } = string.Empty;
    public Address? ShippingAddress { get; private set; }
    public Money TotalValue { get; private set; } = null!;
    public string Notes { get; private set; } = string.Empty;

    private readonly List<OrderLine> _orderLines = new();
    public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

    private Order() { }

    public static Order Create(
        string orderNumber,
        OrderType type,
        Guid warehouseId,
        DateTime expectedDate,
        string customerReference,
        Address? shippingAddress,
        string createdBy
    )
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("Order number cannot be empty", nameof(orderNumber));
        if (warehouseId == Guid.Empty)
            throw new ArgumentException("Warehouse ID cannot be empty", nameof(warehouseId));

        var order = new Order
        {
            OrderNumber = orderNumber,
            Type = type,
            Status = OrderStatus.Pending,
            WarehouseId = warehouseId,
            OrderDate = DateTime.UtcNow,
            ExpectedDate = expectedDate,
            CustomerReference = customerReference,
            ShippingAddress = shippingAddress,
            TotalValue = Money.Create(0, "USD"),
            CreatedBy = createdBy,
        };

        return order;
    }

    public void AddOrderLine(OrderLine orderLine)
    {
        ArgumentNullException.ThrowIfNull(orderLine);

        if (_orderLines.Any(ol => ol.ProductId == orderLine.ProductId))
            throw new InvalidOperationException(
                $"Product {orderLine.ProductId} already exists in this order"
            );

        _orderLines.Add(orderLine);
        RecalculateTotal();
    }

    public void RemoveOrderLine(Guid orderLineId)
    {
        var orderLine = _orderLines.FirstOrDefault(ol => ol.Id == orderLineId);
        if (orderLine == null)
            throw new InvalidOperationException($"Order line {orderLineId} not found");

        _orderLines.Remove(orderLine);
        RecalculateTotal();
    }

    public void Confirm(string updatedBy)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot confirm order in {Status} status");
        if (_orderLines.Count == 0)
            throw new InvalidOperationException("Cannot confirm order without order lines");

        Status = OrderStatus.Confirmed;
        MarkAsUpdated(updatedBy);
    }

    public void StartProcessing(string updatedBy)
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException(
                $"Cannot start processing order in {Status} status"
            );

        Status = OrderStatus.InProgress;
        MarkAsUpdated(updatedBy);
    }

    public void MarkAsPacked(string updatedBy)
    {
        if (Status != OrderStatus.InProgress)
            throw new InvalidOperationException($"Cannot pack order in {Status} status");

        Status = OrderStatus.Packed;
        MarkAsUpdated(updatedBy);
    }

    public void MarkAsShipped(string updatedBy)
    {
        if (Status != OrderStatus.Packed)
            throw new InvalidOperationException($"Cannot ship order in {Status} status");

        Status = OrderStatus.Shipped;
        MarkAsUpdated(updatedBy);
    }

    public void MarkAsDelivered(string updatedBy)
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException($"Cannot deliver order in {Status} status");

        Status = OrderStatus.Delivered;
        CompletedDate = DateTime.UtcNow;
        MarkAsUpdated(updatedBy);
    }

    public void Cancel(string reason, string updatedBy)
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Cannot cancel delivered order");

        Status = OrderStatus.Cancelled;
        Notes = $"Cancelled: {reason}";
        MarkAsUpdated(updatedBy);
    }

    private void RecalculateTotal()
    {
        if (_orderLines.Count == 0)
        {
            TotalValue = Money.Create(0, "USD");
            return;
        }

        var total = _orderLines
            .Select(ol => ol.UnitPrice.Multiply(ol.Quantity))
            .Aggregate((a, b) => a.Add(b));

        TotalValue = total;
    }
}
