using ShopFree.Domain.Enums;

namespace ShopFree.Application.DTOs.PaymentMethods;

public class PaymentMethodDto
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public PaymentMethodType Type { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

