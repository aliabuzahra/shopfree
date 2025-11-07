using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.Stores;
using ShopFree.Domain.Interfaces;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.Stores.Commands.UpdateStore;

public class UpdateStoreCommandHandler : IRequestHandler<UpdateStoreCommand, StoreDto>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateStoreCommandHandler> _logger;
    
    public UpdateStoreCommandHandler(
        IStoreRepository storeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateStoreCommandHandler> logger)
    {
        _storeRepository = storeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<StoreDto> Handle(UpdateStoreCommand request, CancellationToken cancellationToken)
    {
        var store = await _storeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (store == null)
        {
            throw new InvalidOperationException($"Store with ID {request.Id} not found");
        }
        
        store.Update(
            request.Name ?? store.Name,
            request.Description,
            request.LogoUrl);
        
        await _storeRepository.UpdateAsync(store, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<StoreDto>(store);
    }
}

