namespace WMS.Domain.ValueObjects;

public record Quantity
{
    public decimal Value { get; init; }
    public string Unit { get; init; }

    private Quantity(decimal value, string unit)
    {
        Value = value;
        Unit = unit;
    }

    public static Quantity Create(decimal value, string unit)
    {
        if (value < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(value));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be empty", nameof(unit));

        return new Quantity(value, unit);
    }

    public Quantity Add(Quantity other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (Unit != other.Unit)
            throw new InvalidOperationException(
                $"Cannot add quantities with different units: {Unit} and {other.Unit}"
            );

        return new Quantity(Value + other.Value, Unit);
    }

    public Quantity Subtract(Quantity other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (Unit != other.Unit)
            throw new InvalidOperationException(
                $"Cannot subtract quantities with different units: {Unit} and {other.Unit}"
            );

        if (Value < other.Value)
            throw new InvalidOperationException("Result would be negative");

        return new Quantity(Value - other.Value, Unit);
    }

    public override string ToString() => $"{Value} {Unit}";
}
