using ShopFree.Domain.Common;

namespace ShopFree.Domain.Entities;

public class Product : BaseEntity
{
    public int StoreId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public Store Store { get; private set; } = null!;
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Product() { } // For EF Core

    public Product(int storeId, string name, decimal price, string? description = null, string? imageUrl = null, int stock = 0)
    {
        StoreId = storeId;
        Name = name;
        Price = price;
        Description = description;
        ImageUrl = imageUrl;
        Stock = stock;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, decimal price, string? description = null, string? imageUrl = null, int? stock = null)
    {
        Name = name;
        Price = price;
        Description = description;
        ImageUrl = imageUrl;
        if (stock.HasValue)
        {
            Stock = stock.Value;
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStock(int quantity)
    {
        Stock = quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasStock(int requestedQuantity)
    {
        return Stock >= requestedQuantity;
    }
}

