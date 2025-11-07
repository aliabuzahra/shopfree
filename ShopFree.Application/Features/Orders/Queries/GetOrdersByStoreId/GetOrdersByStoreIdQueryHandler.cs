using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.Orders;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.Orders.Queries.GetOrdersByStoreId;

public class GetOrdersByStoreIdQueryHandler : IRequestHandler<GetOrdersByStoreIdQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetOrdersByStoreIdQueryHandler> _logger;

    public GetOrdersByStoreIdQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper,
        ILogger<GetOrdersByStoreIdQueryHandler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersByStoreIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByStoreIdAsync(request.StoreId, cancellationToken);

        // Load items for each order
        var ordersWithItems = new List<Domain.Entities.Order>();
        foreach (var order in orders)
        {
            var orderWithItems = await _orderRepository.GetWithItemsAsync(order.Id, cancellationToken);
            if (orderWithItems != null)
            {
                ordersWithItems.Add(orderWithItems);
            }
        }

        return _mapper.Map<List<OrderDto>>(ordersWithItems);
    }
}

