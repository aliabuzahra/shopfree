using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.Stores;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.Stores.Queries.GetStoresByUserId;

public class GetStoresByUserIdQueryHandler : IRequestHandler<GetStoresByUserIdQuery, List<StoreDto>>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetStoresByUserIdQueryHandler> _logger;
    
    public GetStoresByUserIdQueryHandler(
        IStoreRepository storeRepository,
        IMapper mapper,
        ILogger<GetStoresByUserIdQueryHandler> logger)
    {
        _storeRepository = storeRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<List<StoreDto>> Handle(GetStoresByUserIdQuery request, CancellationToken cancellationToken)
    {
        var stores = await _storeRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        return _mapper.Map<List<StoreDto>>(stores);
    }
}

