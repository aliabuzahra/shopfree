using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.Orders;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.Orders.Queries.GetOrderByOrderNumber;

public class GetOrderByOrderNumberQueryHandler : IRequestHandler<GetOrderByOrderNumberQuery, OrderDto?>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetOrderByOrderNumberQueryHandler> _logger;

    public GetOrderByOrderNumberQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper,
        ILogger<GetOrderByOrderNumberQueryHandler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OrderDto?> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderNumberAsync(request.OrderNumber, cancellationToken);
        if (order == null)
        {
            return null;
        }

        var orderWithItems = await _orderRepository.GetWithItemsAsync(order.Id, cancellationToken);
        return orderWithItems != null ? _mapper.Map<OrderDto>(orderWithItems) : null;
    }
}

