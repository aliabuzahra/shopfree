using Microsoft.EntityFrameworkCore;
using ShopFree.Domain.Entities;
using ShopFree.Domain.Interfaces.Repositories;
using ShopFree.Infrastructure.Data;

namespace ShopFree.Infrastructure.Repositories;

public class PaymentMethodRepository : BaseRepository<PaymentMethod>, IPaymentMethodRepository
{
    public PaymentMethodRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<List<PaymentMethod>> GetByStoreIdAsync(int storeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(pm => pm.StoreId == storeId).ToListAsync(cancellationToken);
    }
    
    public async Task<List<PaymentMethod>> GetActiveByStoreIdAsync(int storeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(pm => pm.StoreId == storeId && pm.IsActive).ToListAsync(cancellationToken);
    }
}

