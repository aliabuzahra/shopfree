using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.PaymentMethods;
using ShopFree.Domain.Entities;
using ShopFree.Domain.Interfaces;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;

public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommand, PaymentMethodDto>
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreatePaymentMethodCommandHandler> _logger;
    
    public CreatePaymentMethodCommandHandler(
        IPaymentMethodRepository paymentMethodRepository,
        IStoreRepository storeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreatePaymentMethodCommandHandler> logger)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _storeRepository = storeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<PaymentMethodDto> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        // Verify store exists
        var store = await _storeRepository.GetByIdAsync(request.StoreId, cancellationToken);
        if (store == null)
        {
            throw new InvalidOperationException($"Store with ID {request.StoreId} not found");
        }
        
        var paymentMethod = new PaymentMethod(
            request.StoreId,
            request.Type,
            request.Title,
            request.Details);
        
        await _paymentMethodRepository.AddAsync(paymentMethod, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<PaymentMethodDto>(paymentMethod);
    }
}

