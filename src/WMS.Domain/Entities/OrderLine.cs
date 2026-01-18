using WMS.Domain.Common;
using WMS.Domain.ValueObjects;

namespace WMS.Domain.Entities;

public class OrderLine : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = null!;
    public Money LineTotal => UnitPrice.Multiply(Quantity);
    public int QuantityPicked { get; private set; }
    public int QuantityShipped { get; private set; }

    private OrderLine() { }

    public static OrderLine Create(
        Guid orderId,
        Guid productId,
        int quantity,
        Money unitPrice,
        string createdBy
    )
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("Order ID cannot be empty", nameof(orderId));
        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty", nameof(productId));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        var orderLine = new OrderLine
        {
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            QuantityPicked = 0,
            QuantityShipped = 0,
            CreatedBy = createdBy,
        };

        return orderLine;
    }

    public void UpdateQuantity(int quantity, string updatedBy)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        Quantity = quantity;
        MarkAsUpdated(updatedBy);
    }

    public void PickQuantity(int quantity, string updatedBy)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (QuantityPicked + quantity > Quantity)
            throw new InvalidOperationException("Cannot pick more than ordered quantity");

        QuantityPicked += quantity;
        MarkAsUpdated(updatedBy);
    }

    public void ShipQuantity(int quantity, string updatedBy)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (QuantityShipped + quantity > QuantityPicked)
            throw new InvalidOperationException("Cannot ship more than picked quantity");

        QuantityShipped += quantity;
        MarkAsUpdated(updatedBy);
    }
}
