using Microsoft.EntityFrameworkCore;
using ShopFree.Domain.Entities;
using ShopFree.Domain.Interfaces.Repositories;
using ShopFree.Infrastructure.Data;

namespace ShopFree.Infrastructure.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<List<Order>> GetByStoreIdAsync(int storeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.StoreId == storeId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}

