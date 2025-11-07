using MediatR;
using ShopFree.Application.DTOs.Orders;

namespace ShopFree.Application.Features.Orders.Queries.GetOrdersByStoreId;

public class GetOrdersByStoreIdQuery : IRequest<List<OrderDto>>
{
    public int StoreId { get; set; }
}

