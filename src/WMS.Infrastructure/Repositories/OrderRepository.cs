using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(WMSDbContext context)
        : base(context) { }

    public async Task<Order?> GetByOrderNumberAsync(
        string orderNumber,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet
            .Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByWarehouseAsync(
        Guid warehouseId,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet
            .Include(o => o.OrderLines)
            .Where(o => o.WarehouseId == warehouseId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(
        OrderStatus status,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet
            .Include(o => o.OrderLines)
            .Where(o => o.Status == status)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Order?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet
            .Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}
