using MediatR;
using ShopFree.Application.DTOs.PaymentMethods;
using ShopFree.Domain.Enums;

namespace ShopFree.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;

public class CreatePaymentMethodCommand : IRequest<PaymentMethodDto>
{
    public int StoreId { get; set; }
    public PaymentMethodType Type { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
}

