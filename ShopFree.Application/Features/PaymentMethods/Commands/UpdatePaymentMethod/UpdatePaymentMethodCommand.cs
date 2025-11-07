using MediatR;
using ShopFree.Application.DTOs.PaymentMethods;

namespace ShopFree.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod;

public class UpdatePaymentMethodCommand : IRequest<PaymentMethodDto>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public bool? IsActive { get; set; }
}

