using MediatR;
using ShopFree.Application.DTOs.PaymentMethods;

namespace ShopFree.Application.Features.PaymentMethods.Queries.GetPaymentMethodsByStoreId;

public class GetPaymentMethodsByStoreIdQuery : IRequest<List<PaymentMethodDto>>
{
    public int StoreId { get; set; }
    public bool ActiveOnly { get; set; } = false;
}

