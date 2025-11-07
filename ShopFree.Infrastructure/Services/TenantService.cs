using ShopFree.Domain.Interfaces;

namespace ShopFree.Infrastructure.Services;

public class TenantService : ITenantService
{
    private int? _currentStoreId;
    private string? _currentSubdomain;
    
    public int? GetCurrentStoreId()
    {
        return _currentStoreId;
    }
    
    public void SetCurrentStoreId(int storeId)
    {
        _currentStoreId = storeId;
    }
    
    public string? GetCurrentSubdomain()
    {
        return _currentSubdomain;
    }
    
    public void SetCurrentSubdomain(string subdomain)
    {
        _currentSubdomain = subdomain;
    }
}

