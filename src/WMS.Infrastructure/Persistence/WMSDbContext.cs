using Microsoft.EntityFrameworkCore;
using WMS.Domain.Common;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistence;

public class WMSDbContext : DbContext
{
    public WMSDbContext(DbContextOptions<WMSDbContext> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Sku).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).HasMaxLength(100);

            entity.OwnsOne(
                e => e.Price,
                price =>
                {
                    price
                        .Property(p => p.Amount)
                        .HasColumnName("Price")
                        .HasColumnType("decimal(18,2)");
                    price.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3);
                }
            );

            entity.OwnsOne(
                e => e.Weight,
                weight =>
                {
                    weight
                        .Property(w => w.Value)
                        .HasColumnName("Weight")
                        .HasColumnType("decimal(18,2)");
                    weight.Property(w => w.Unit).HasColumnName("WeightUnit").HasMaxLength(10);
                }
            );

            entity.OwnsOne(
                e => e.Dimensions,
                dimensions =>
                {
                    dimensions
                        .Property(d => d.Value)
                        .HasColumnName("Dimensions")
                        .HasColumnType("decimal(18,2)");
                    dimensions
                        .Property(d => d.Unit)
                        .HasColumnName("DimensionsUnit")
                        .HasMaxLength(10);
                }
            );

            entity.HasIndex(e => e.Sku).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Warehouse configuration
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ContactPhone).HasMaxLength(20);
            entity.Property(e => e.ContactEmail).HasMaxLength(100);

            entity.OwnsOne(
                e => e.Address,
                address =>
                {
                    address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200);
                    address.Property(a => a.City).HasColumnName("City").HasMaxLength(100);
                    address.Property(a => a.State).HasColumnName("State").HasMaxLength(100);
                    address
                        .Property(a => a.PostalCode)
                        .HasColumnName("PostalCode")
                        .HasMaxLength(20);
                    address.Property(a => a.Country).HasColumnName("Country").HasMaxLength(100);
                }
            );

            entity
                .HasMany(w => w.Locations)
                .WithOne()
                .HasForeignKey(l => l.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Location configuration
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Aisle).HasMaxLength(20);
            entity.Property(e => e.Shelf).HasMaxLength(20);
            entity.Property(e => e.Bin).HasMaxLength(20);

            entity.HasIndex(e => new { e.WarehouseId, e.Code }).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Inventory configuration
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity
                .HasIndex(e => new
                {
                    e.ProductId,
                    e.WarehouseId,
                    e.LocationId,
                })
                .IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CustomerReference).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.Status).HasConversion<string>();

            entity.OwnsOne(
                e => e.TotalValue,
                totalValue =>
                {
                    totalValue
                        .Property(t => t.Amount)
                        .HasColumnName("TotalValue")
                        .HasColumnType("decimal(18,2)");
                    totalValue.Property(t => t.Currency).HasColumnName("Currency").HasMaxLength(3);
                }
            );

            entity.OwnsOne(
                e => e.ShippingAddress,
                address =>
                {
                    address
                        .Property(a => a.Street)
                        .HasColumnName("ShippingStreet")
                        .HasMaxLength(200);
                    address.Property(a => a.City).HasColumnName("ShippingCity").HasMaxLength(100);
                    address.Property(a => a.State).HasColumnName("ShippingState").HasMaxLength(100);
                    address
                        .Property(a => a.PostalCode)
                        .HasColumnName("ShippingPostalCode")
                        .HasMaxLength(20);
                    address
                        .Property(a => a.Country)
                        .HasColumnName("ShippingCountry")
                        .HasMaxLength(100);
                }
            );

            entity
                .HasMany(o => o.OrderLines)
                .WithOne()
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.OrderNumber).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // OrderLine configuration
        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.OwnsOne(
                e => e.UnitPrice,
                unitPrice =>
                {
                    unitPrice
                        .Property(u => u.Amount)
                        .HasColumnName("UnitPrice")
                        .HasColumnType("decimal(18,2)");
                    unitPrice.Property(u => u.Currency).HasColumnName("Currency").HasMaxLength(3);
                }
            );

            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var entities = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        foreach (var entity in entities)
        {
            // In a real application, we would dispatch these events to a message bus
            entity.ClearDomainEvents();
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
