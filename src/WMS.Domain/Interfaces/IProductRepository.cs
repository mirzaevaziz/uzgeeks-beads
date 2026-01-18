using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default
    );
    Task<bool> ExistsAsync(string sku, CancellationToken cancellationToken = default);
}
