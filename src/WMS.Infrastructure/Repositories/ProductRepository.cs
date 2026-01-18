using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(WMSDbContext context)
        : base(context) { }

    public async Task<Product?> GetBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet.FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet.Where(p => p.Category == category).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(p => p.Sku == sku, cancellationToken);
    }
}
