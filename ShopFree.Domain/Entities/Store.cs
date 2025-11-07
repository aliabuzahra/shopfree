using ShopFree.Domain.Common;

namespace ShopFree.Domain.Entities;

public class Store : BaseEntity
{
    public int UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Subdomain { get; private set; }
    public string? LogoUrl { get; private set; }

    // Navigation properties
    public User User { get; private set; } = null!;
    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private readonly List<Order> _orders = new();
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    private readonly List<PaymentMethod> _paymentMethods = new();
    public IReadOnlyCollection<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    private Store() { } // For EF Core

    public Store(int userId, string name, string? description = null, string? subdomain = null, string? logoUrl = null)
    {
        UserId = userId;
        Name = name;
        Description = description;
        Subdomain = subdomain;
        LogoUrl = logoUrl;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description = null, string? logoUrl = null)
    {
        Name = name;
        Description = description;
        LogoUrl = logoUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSubdomain(string subdomain)
    {
        Subdomain = subdomain;
        UpdatedAt = DateTime.UtcNow;
    }
}

