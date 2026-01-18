using WMS.Domain.Common;
using WMS.Domain.ValueObjects;

namespace WMS.Domain.Entities;

public class Product : BaseEntity
{
    public string Sku { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Price { get; private set; } = null!;
    public Quantity Weight { get; private set; } = null!;
    public Quantity Dimensions { get; private set; } = null!;
    public int ReorderLevel { get; private set; }
    public int ReorderQuantity { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private Product() { }

    public static Product Create(
        string sku,
        string name,
        string description,
        Money price,
        Quantity weight,
        Quantity dimensions,
        int reorderLevel,
        int reorderQuantity,
        string category,
        string createdBy
    )
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU cannot be empty", nameof(sku));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (reorderLevel < 0)
            throw new ArgumentException("Reorder level cannot be negative", nameof(reorderLevel));
        if (reorderQuantity < 0)
            throw new ArgumentException(
                "Reorder quantity cannot be negative",
                nameof(reorderQuantity)
            );

        var product = new Product
        {
            Sku = sku,
            Name = name,
            Description = description,
            Price = price,
            Weight = weight,
            Dimensions = dimensions,
            ReorderLevel = reorderLevel,
            ReorderQuantity = reorderQuantity,
            Category = category,
            IsActive = true,
            CreatedBy = createdBy,
        };

        return product;
    }

    public void UpdateDetails(string name, string description, Money price, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        Description = description;
        Price = price;
        MarkAsUpdated(updatedBy);
    }

    public void UpdateReorderSettings(int reorderLevel, int reorderQuantity, string updatedBy)
    {
        if (reorderLevel < 0)
            throw new ArgumentException("Reorder level cannot be negative", nameof(reorderLevel));
        if (reorderQuantity < 0)
            throw new ArgumentException(
                "Reorder quantity cannot be negative",
                nameof(reorderQuantity)
            );

        ReorderLevel = reorderLevel;
        ReorderQuantity = reorderQuantity;
        MarkAsUpdated(updatedBy);
    }

    public void Activate(string updatedBy)
    {
        IsActive = true;
        MarkAsUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        MarkAsUpdated(updatedBy);
    }
}
