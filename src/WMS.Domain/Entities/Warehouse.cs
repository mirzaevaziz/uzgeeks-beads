using WMS.Domain.Common;
using WMS.Domain.ValueObjects;

namespace WMS.Domain.Entities;

public class Warehouse : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public Address Address { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public string ContactPhone { get; private set; } = string.Empty;
    public string ContactEmail { get; private set; } = string.Empty;

    private readonly List<Location> _locations = new();
    public IReadOnlyCollection<Location> Locations => _locations.AsReadOnly();

    private Warehouse() { }

    public static Warehouse Create(
        string code,
        string name,
        Address address,
        string contactPhone,
        string contactEmail,
        string createdBy
    )
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        var warehouse = new Warehouse
        {
            Code = code,
            Name = name,
            Address = address,
            ContactPhone = contactPhone,
            ContactEmail = contactEmail,
            IsActive = true,
            CreatedBy = createdBy,
        };

        return warehouse;
    }

    public void AddLocation(Location location)
    {
        ArgumentNullException.ThrowIfNull(location);

        if (_locations.Any(l => l.Code == location.Code))
            throw new InvalidOperationException(
                $"Location with code {location.Code} already exists"
            );

        _locations.Add(location);
    }

    public void UpdateDetails(
        string name,
        Address address,
        string contactPhone,
        string contactEmail,
        string updatedBy
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        Address = address;
        ContactPhone = contactPhone;
        ContactEmail = contactEmail;
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
