using Microsoft.EntityFrameworkCore;
using ShopFree.Domain.Entities;
using ShopFree.Domain.Interfaces.Repositories;
using ShopFree.Infrastructure.Data;

namespace ShopFree.Infrastructure.Repositories;

public class StoreRepository : BaseRepository<Store>, IStoreRepository
{
    public StoreRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Store?> GetBySubdomainAsync(string subdomain, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.Subdomain == subdomain, cancellationToken);
    }

    public async Task<List<Store>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(s => s.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<bool> SubdomainExistsAsync(string subdomain, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(s => s.Subdomain == subdomain, cancellationToken);
    }
}

