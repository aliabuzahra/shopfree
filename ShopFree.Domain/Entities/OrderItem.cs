using ShopFree.Domain.Common;

namespace ShopFree.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice { get; private set; }

    // Navigation properties
    public Order Order { get; private set; } = null!;
    public Product Product { get; private set; } = null!;

    private OrderItem() { } // For EF Core

    public OrderItem(int orderId, int productId, int quantity, decimal unitPrice)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = quantity * unitPrice;
        CreatedAt = DateTime.UtcNow;
    }
}

