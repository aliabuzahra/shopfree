using ShopFree.Domain.Entities;

namespace ShopFree.Domain.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
    Task<List<Product>> GetActiveByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
}

