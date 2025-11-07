using MediatR;
using ShopFree.Application.DTOs.Orders;

namespace ShopFree.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<OrderDto?>
{
    public int Id { get; set; }
}

