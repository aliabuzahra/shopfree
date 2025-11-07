using ShopFree.Domain.Entities;

namespace ShopFree.Domain.Interfaces.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<List<Order>> GetByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
    Task<Order?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default);
}

