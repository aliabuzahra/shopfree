using MediatR;
using ShopFree.Application.DTOs.Orders;

namespace ShopFree.Application.Features.Orders.Queries.GetOrderByOrderNumber;

public class GetOrderByOrderNumberQuery : IRequest<OrderDto?>
{
    public string OrderNumber { get; set; } = string.Empty;
}

