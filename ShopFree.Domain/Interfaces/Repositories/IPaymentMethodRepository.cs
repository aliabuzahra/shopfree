using ShopFree.Domain.Entities;
using ShopFree.Domain.Enums;

namespace ShopFree.Domain.Interfaces.Repositories;

public interface IPaymentMethodRepository : IRepository<PaymentMethod>
{
    Task<List<PaymentMethod>> GetByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
    Task<List<PaymentMethod>> GetActiveByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
}

