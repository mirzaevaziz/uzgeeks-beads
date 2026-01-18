using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByOrderNumberAsync(
        string orderNumber,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Order>> GetByWarehouseAsync(
        Guid warehouseId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Order>> GetByStatusAsync(
        OrderStatus status,
        CancellationToken cancellationToken = default
    );
}
