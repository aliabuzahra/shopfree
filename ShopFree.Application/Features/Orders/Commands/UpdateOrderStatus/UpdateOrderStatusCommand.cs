using MediatR;
using ShopFree.Application.DTOs.Orders;
using ShopFree.Domain.Enums;

namespace ShopFree.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommand : IRequest<OrderDto>
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
}

