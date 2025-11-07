using ShopFree.Domain.Common;
using ShopFree.Domain.Enums;

namespace ShopFree.Domain.Entities;

public class PaymentMethod : BaseEntity
{
    public int StoreId { get; private set; }
    public PaymentMethodType Type { get; private set; }
    public string? Title { get; private set; }
    public string? Details { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public Store Store { get; private set; } = null!;

    private PaymentMethod() { } // For EF Core

    public PaymentMethod(int storeId, PaymentMethodType type, string? title = null, string? details = null)
    {
        StoreId = storeId;
        Type = type;
        Title = title;
        Details = details;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string? title = null, string? details = null)
    {
        Title = title;
        Details = details;
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
}

