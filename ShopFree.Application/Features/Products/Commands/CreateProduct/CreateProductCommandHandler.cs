using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.Products;
using ShopFree.Domain.Entities;
using ShopFree.Domain.Interfaces;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductCommandHandler> _logger;
    
    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IStoreRepository storeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _storeRepository = storeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Verify store exists
        var store = await _storeRepository.GetByIdAsync(request.StoreId, cancellationToken);
        if (store == null)
        {
            throw new InvalidOperationException($"Store with ID {request.StoreId} not found");
        }
        
        var product = new Product(
            request.StoreId,
            request.Name,
            request.Price,
            request.Description,
            request.ImageUrl,
            request.Stock);
        
        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ProductDto>(product);
    }
}

