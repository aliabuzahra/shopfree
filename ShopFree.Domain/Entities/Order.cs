using ShopFree.Domain.Common;
using ShopFree.Domain.Enums;
using ShopFree.Domain.ValueObjects;

namespace ShopFree.Domain.Entities;

public class Order : BaseEntity
{
    public int StoreId { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public CustomerInfo Customer { get; private set; } = null!;
    public Address ShippingAddress { get; private set; } = null!;
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public PaymentMethodType PaymentMethodType { get; private set; }
    public string? PaymentDetails { get; private set; }
    public string? Notes { get; private set; }
    
    // Navigation properties
    public Store Store { get; private set; } = null!;
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    
    private Order() { } // For EF Core
    
    public Order(
        int storeId,
        string orderNumber,
        CustomerInfo customer,
        Address shippingAddress,
        PaymentMethodType paymentMethodType,
        string? paymentDetails = null,
        string? notes = null)
    {
        StoreId = storeId;
        OrderNumber = orderNumber;
        Customer = customer;
        ShippingAddress = shippingAddress;
        PaymentMethodType = paymentMethodType;
        PaymentDetails = paymentDetails;
        Notes = notes;
        Status = OrderStatus.Pending;
        TotalAmount = 0;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void AddOrderItem(OrderItem item)
    {
        _orderItems.Add(item);
        RecalculateTotal();
    }
    
    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
    
    private void RecalculateTotal()
    {
        TotalAmount = _orderItems.Sum(item => item.TotalPrice);
    }
}

