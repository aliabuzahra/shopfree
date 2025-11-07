using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.Products;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.Products.Queries.GetProductsByStoreId;

public class GetProductsByStoreIdQueryHandler : IRequestHandler<GetProductsByStoreIdQuery, List<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductsByStoreIdQueryHandler> _logger;

    public GetProductsByStoreIdQueryHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<GetProductsByStoreIdQueryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ProductDto>> Handle(GetProductsByStoreIdQuery request, CancellationToken cancellationToken)
    {
        var products = request.ActiveOnly
            ? await _productRepository.GetActiveByStoreIdAsync(request.StoreId, cancellationToken)
            : await _productRepository.GetByStoreIdAsync(request.StoreId, cancellationToken);

        return _mapper.Map<List<ProductDto>>(products);
    }
}

