using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.Stores;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.Stores.Queries.GetStoreById;

public class GetStoreByIdQueryHandler : IRequestHandler<GetStoreByIdQuery, StoreDto?>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetStoreByIdQueryHandler> _logger;

    public GetStoreByIdQueryHandler(
        IStoreRepository storeRepository,
        IMapper mapper,
        ILogger<GetStoreByIdQueryHandler> logger)
    {
        _storeRepository = storeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StoreDto?> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
    {
        var store = await _storeRepository.GetByIdAsync(request.Id, cancellationToken);
        return store != null ? _mapper.Map<StoreDto>(store) : null;
    }
}

