namespace WMS.Application.DTOs;

public record ProductDto
{
    public Guid Id { get; init; }
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Currency { get; init; } = string.Empty;
    public decimal Weight { get; init; }
    public string WeightUnit { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public int ReorderLevel { get; init; }
    public int ReorderQuantity { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateProductDto
{
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Currency { get; init; } = "USD";
    public decimal Weight { get; init; }
    public string WeightUnit { get; init; } = "kg";
    public decimal Dimensions { get; init; }
    public string DimensionsUnit { get; init; } = "cm³";
    public string Category { get; init; } = string.Empty;
    public int ReorderLevel { get; init; }
    public int ReorderQuantity { get; init; }
}

public record UpdateProductDto
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Currency { get; init; } = "USD";
}
