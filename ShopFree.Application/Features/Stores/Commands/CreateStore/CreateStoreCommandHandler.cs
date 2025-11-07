using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.Stores;
using ShopFree.Domain.Entities;
using ShopFree.Domain.Interfaces;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.Stores.Commands.CreateStore;

public class CreateStoreCommandHandler : IRequestHandler<CreateStoreCommand, StoreDto>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateStoreCommandHandler> _logger;
    
    public CreateStoreCommandHandler(
        IStoreRepository storeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateStoreCommandHandler> logger)
    {
        _storeRepository = storeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<StoreDto> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
    {
        // Check if subdomain is already taken
        if (!string.IsNullOrEmpty(request.Subdomain))
        {
            if (await _storeRepository.SubdomainExistsAsync(request.Subdomain, cancellationToken))
            {
                throw new InvalidOperationException($"Subdomain '{request.Subdomain}' is already taken");
            }
        }
        
        var store = new Store(
            request.UserId,
            request.Name,
            request.Description,
            request.Subdomain,
            request.LogoUrl);
        
        await _storeRepository.AddAsync(store, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<StoreDto>(store);
    }
}

