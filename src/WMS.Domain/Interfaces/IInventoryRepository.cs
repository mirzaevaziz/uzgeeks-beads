using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces;

public interface IInventoryRepository : IRepository<Inventory>
{
    Task<Inventory?> GetByProductAndWarehouseAsync(
        Guid productId,
        Guid warehouseId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Inventory>> GetByWarehouseAsync(
        Guid warehouseId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Inventory>> GetLowStockItemsAsync(
        int threshold,
        CancellationToken cancellationToken = default
    );
}
