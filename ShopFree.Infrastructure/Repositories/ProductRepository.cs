using Microsoft.EntityFrameworkCore;
using ShopFree.Domain.Entities;
using ShopFree.Domain.Interfaces.Repositories;
using ShopFree.Infrastructure.Data;

namespace ShopFree.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<List<Product>> GetByStoreIdAsync(int storeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => p.StoreId == storeId).ToListAsync(cancellationToken);
    }
    
    public async Task<List<Product>> GetActiveByStoreIdAsync(int storeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => p.StoreId == storeId && p.IsActive).ToListAsync(cancellationToken);
    }
}

