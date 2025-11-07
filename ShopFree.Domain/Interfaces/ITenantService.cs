namespace ShopFree.Domain.Interfaces;

public interface ITenantService
{
    int? GetCurrentStoreId();
    void SetCurrentStoreId(int storeId);
    string? GetCurrentSubdomain();
    void SetCurrentSubdomain(string subdomain);
}

