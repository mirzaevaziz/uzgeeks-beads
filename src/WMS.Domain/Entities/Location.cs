using WMS.Domain.Common;

namespace WMS.Domain.Entities;

public class Location : BaseEntity
{
    public Guid WarehouseId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Aisle { get; private set; } = string.Empty;
    public string Shelf { get; private set; } = string.Empty;
    public string Bin { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public int Capacity { get; private set; }
    public int CurrentOccupancy { get; private set; }

    private Location() { }

    public static Location Create(
        Guid warehouseId,
        string code,
        string aisle,
        string shelf,
        string bin,
        int capacity,
        string createdBy
    )
    {
        if (warehouseId == Guid.Empty)
            throw new ArgumentException("Warehouse ID cannot be empty", nameof(warehouseId));
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty", nameof(code));
        if (capacity < 0)
            throw new ArgumentException("Capacity cannot be negative", nameof(capacity));

        var location = new Location
        {
            WarehouseId = warehouseId,
            Code = code,
            Aisle = aisle,
            Shelf = shelf,
            Bin = bin,
            Capacity = capacity,
            CurrentOccupancy = 0,
            IsActive = true,
            CreatedBy = createdBy,
        };

        return location;
    }

    public bool CanAccommodate(int quantity)
    {
        return IsActive && (CurrentOccupancy + quantity <= Capacity);
    }

    public void IncreaseOccupancy(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));
        if (!CanAccommodate(quantity))
            throw new InvalidOperationException("Not enough capacity");

        CurrentOccupancy += quantity;
    }

    public void DecreaseOccupancy(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));
        if (CurrentOccupancy < quantity)
            throw new InvalidOperationException("Occupancy cannot be negative");

        CurrentOccupancy -= quantity;
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
