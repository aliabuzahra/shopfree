using ShopFree.Domain.Entities;

namespace ShopFree.Domain.Interfaces.Repositories;

public interface IStoreRepository : IRepository<Store>
{
    Task<Store?> GetBySubdomainAsync(string subdomain, CancellationToken cancellationToken = default);
    Task<List<Store>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> SubdomainExistsAsync(string subdomain, CancellationToken cancellationToken = default);
}

