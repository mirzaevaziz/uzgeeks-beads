using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class InventoryRepository : Repository<Inventory>, IInventoryRepository
{
    public InventoryRepository(WMSDbContext context)
        : base(context) { }

    public async Task<Inventory?> GetByProductAndWarehouseAsync(
        Guid productId,
        Guid warehouseId,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet.FirstOrDefaultAsync(
            i => i.ProductId == productId && i.WarehouseId == warehouseId,
            cancellationToken
        );
    }

    public async Task<IEnumerable<Inventory>> GetByWarehouseAsync(
        Guid warehouseId,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet.Where(i => i.WarehouseId == warehouseId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Inventory>> GetLowStockItemsAsync(
        int threshold,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet
            .Where(i => i.QuantityAvailable <= threshold)
            .ToListAsync(cancellationToken);
    }
}
