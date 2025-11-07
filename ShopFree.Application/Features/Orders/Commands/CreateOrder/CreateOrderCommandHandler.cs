using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.Orders;
using ShopFree.Domain.Entities;
using ShopFree.Domain.Enums;
using ShopFree.Domain.Interfaces;
using ShopFree.Domain.Interfaces.Repositories;
using ShopFree.Domain.ValueObjects;

namespace ShopFree.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    
    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IStoreRepository storeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _storeRepository = storeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Verify store exists
        var store = await _storeRepository.GetByIdAsync(request.StoreId, cancellationToken);
        if (store == null)
        {
            throw new InvalidOperationException($"Store with ID {request.StoreId} not found");
        }
        
        // Generate order number
        var orderNumber = GenerateOrderNumber();
        
        // Create customer info
        var customer = new CustomerInfo(
            request.CustomerName,
            request.CustomerEmail,
            request.CustomerPhone);
        
        // Create shipping address (simplified - using full address string)
        var shippingAddress = Address.FromFullAddress(request.ShippingAddress);
        
        // Validate products and check stock before creating order
        var productsToUpdate = new List<(Product product, int quantity)>();
        foreach (var itemDto in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId, cancellationToken);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {itemDto.ProductId} not found");
            }
            
            if (!product.IsActive)
            {
                throw new InvalidOperationException($"Product {product.Name} is not active");
            }
            
            if (!product.HasStock(itemDto.Quantity))
            {
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
            }
            
            productsToUpdate.Add((product, itemDto.Quantity));
        }
        
        // Create order
        var order = new Order(
            request.StoreId,
            orderNumber,
            customer,
            shippingAddress,
            request.PaymentMethodType,
            request.PaymentDetails,
            request.Notes);
        
        // Save order first to get ID
        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Add order items
        foreach (var (product, quantity) in productsToUpdate)
        {
            var orderItem = new OrderItem(
                order.Id,
                product.Id,
                quantity,
                product.Price);
            
            order.AddOrderItem(orderItem);
            
            // Update product stock
            product.UpdateStock(product.Stock - quantity);
            await _productRepository.UpdateAsync(product, cancellationToken);
        }
        
        // Update order with items
        await _orderRepository.UpdateAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Reload order with items
        var createdOrder = await _orderRepository.GetWithItemsAsync(order.Id, cancellationToken);
        return _mapper.Map<OrderDto>(createdOrder);
    }
    
    private string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
    }
}

